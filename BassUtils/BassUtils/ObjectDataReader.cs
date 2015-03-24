using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// This class is based on the EntityDataReader from Microsoft, which can be
// found at http://code.msdn.microsoft.com/LinqEntityDataReader.
// It has been extensively modified (mainly simplified).
//
// Differences/Improvements
// ========================
// * Renamed from EntityDataReader to ObjectDataReader as it will now work with
//   any type of .Net object (POCOs).
// * Does not use/need/reference the EntityFramework.
// * Returns public fields and enumerations (EntityDataReader just did properties).
// * Handles a few more types (unsigned integral types, DateTimeOffset etc.)
// * The ability to flatten related objects has been removed. It only worked for one-level,
//   and was EF specific. If you want to flatten object graphs, use LINQ.
// * The use of an "ordering constructor" to determine the order of columns in the reader
//   was removed. An IDataReader is a relational data construct, the order of columns
//   should be irrelevant.
// * As a consequence of the above changes, there is no longer any need for the
//   EntityDataReaderOptions class and it has been deleted. The only remaining option is
//   the TreatNullables enum (an enum is much clearer than a bool).
// * Code and comments have been tidied.
// * Some UnitTests written.
//
// TODO
// ====
// I am not sure if the TreatNullables enum should be kept. The conversion of
// System.Nullable values to DBNull.Value could be made to work for all data
// readers by creating a class NullConvertingDataReader which you use to wrap
// any type of data reader.

// http://www.codeproject.com/Articles/14560/Fast-Dynamic-Property-Field-Accessors
// http://rogeralsing.com/2008/02/26/linq-expressions-access-private-fields/

#region Microsoft Original Header
//===============================================================================
// Copyright © 2008 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
//
//This code lives here: http://code.msdn.microsoft.com/LinqEntityDataReader
//Please visit for comments, issues and updates
//
//Version 1.0.0.1 Added GetSchemaTable support for Loading DataTables from EntityDataReader
//Version 1.0.0.2 Added support for Entity Framework types, including Foreign Key columns
//Version 1.0.0.3 In DataReader.GetValue, now using dynamic methods for common scalar types instead of reflection with PropertyInfo.GetValue()
//Version 1.0.0.4 Added support for simple IEnumerable<T> where T is a scalar to support, eg, passing List<int> to a TVP
//Version 1.0.0.5 Simplified the Attribute code, added dynamic method support for all scalar types
//Version 1.0.0.6 Replaced the switch block for ValueAccessor with a Lambda Expression
//Version 1.0.0.6 Fixed a bug with nullable foreign keys on EF entities
//Version 1.0.0.6 Extensive refactoring, introduced EntityDataReaderOptions, changed constructors
//Version 1.0.0.6 Introduced option to flatten related entities.  Now you can have the EntityDataReader flatten an object graph.
//                This is especially useful for enabling you to project all the scalars of a related enty by just projecting the entity.
//                eg. a projection like new { salesOrder.ID, salesOrder.Customer, salesOrder.Product }                 
//                will create a DbDataReader with Id, Customer.*, Product.*
//Version 1.0.0.7 For anonymous types the order of columns is now controlled by the anonymous type's constructor arguments, for which 
//                Reflection tracks the ordinal position.  This ordering is applied to any type where the constructor args match the properties
//                1-1 on name and type.  Reflection over properties does not preserve the declaration order of the properties, and for Table Valued Parameters
//                SqlClient maps the DbDataReader columns to the TVP columns by ordinal position, not by name.  This relies on the behavior of 
//                the C# compiler that Anonymous types have a constructor with constructor arguments that match the object initializer on both name
//                and position.
#endregion

namespace BassUtils
{
    /// <summary>
	/// The ObjectDataReader presents a collection of CLR objects via an IDataReader interface.
    /// Scalar properties, fields and enumerations are projected. They can be of System.Nullable,
    /// or value types.
    /// 
    /// This is useful for doing high-speed data loads with SqlBulkCopy, and copying collections
    /// of entities to a DataTable for use with SQL Server Table-Valued parameters, or for interop
    /// with older ADO.NET applications.
    /// 
    /// For explicit control over the fields projected by the IDataReader, just wrap your collection
    /// of entities in an anonymous type projection before wrapping it in an ObjectDataReader.
    /// i.e. use a LINQ query. This is also the way to flatten object graphs.
    /// </summary>
    /// <typeparam name="T">The type of things we are creating a reader over</typeparam>
    public sealed class ObjectDataReader<T> : IDataReader
    {
        static List<Attribute> scalarAttributes;

        readonly NullConversion NullConversion;
        readonly IEnumerator<T> Enumerator;
        readonly List<Attribute> Attributes;
        T current;
        bool closed;

        class Attribute
        {
            public readonly Type Type;
            public readonly string Name;
            readonly Func<T, object> ValueAccessor;

            /// <summary>
            /// Uses Lamda expressions to create a Func&lt;T,object&gt; that invokes the given property getter.
            /// The property value will be extracted and cast to type TProperty
            /// </summary>
            /// <typeparam name="TObject">The type of the object declaring the property.</typeparam>
            /// <typeparam name="TProperty">The type to cast the property value to</typeparam>
            /// <param name="pi">PropertyInfo pointing to the property to wrap</param>
            /// <returns></returns>
            static Func<TObject, TProperty> MakePropertyAccessor<TObject, TProperty>(PropertyInfo pi)
            {
                ParameterExpression objParam = Expression.Parameter(typeof(TObject), "obj");
                MemberExpression typedAccessor = Expression.PropertyOrField(objParam, pi.Name);
                UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
                LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, TProperty>>(castToObject, objParam);

                return (Func<TObject, TProperty>)lambdaExpr.Compile();
            }

            static Func<TObject, TField> MakeFieldAccessor<TObject, TField>(FieldInfo fi)
            {
                ParameterExpression objParam = Expression.Parameter(typeof(TObject), "arg");
                MemberExpression typedAccessor = Expression.Field(objParam, fi.Name);
                UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
                LambdaExpression lambda = Expression.Lambda(typeof(Func<TObject, TField>), castToObject, objParam);
                Func<TObject, TField> compiled = (Func<TObject, TField>)lambda.Compile();

                return compiled;
            }

            public Attribute(FieldInfo fi)
            {
                Name = fi.Name;
                Type = fi.FieldType;
                ValueAccessor = MakeFieldAccessor<T, object>(fi);
            }

            public Attribute(PropertyInfo pi)
            {
                Name = pi.Name;
                Type = pi.PropertyType;
                ValueAccessor = MakePropertyAccessor<T, object>(pi);
            }

            public Attribute(string name, Type type, Func<T, object> getValue)
            {
                Name = name;
                Type = type;
                ValueAccessor = getValue;
            }

            public object GetValue(T target)
            {
                return ValueAccessor(target);
            }
        }

        static readonly HashSet<Type> scalarTypes = new HashSet<Type>() 
            { 
            // Reference types.
            typeof(String),
            typeof(Byte[]),
            // Value types. Nullable versions of these are also considered scalar types.
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Guid),
            typeof(Boolean),
            typeof(TimeSpan),
            };

        static bool IsNullableType(Type t)
        {
            var t2 = Nullable.GetUnderlyingType(t);
            return t2 != null;
        }

        static bool IsScalarType(Type t)
        {
            if (scalarTypes.Contains(t) || t.IsEnum)
                return true;

            var t2 = Nullable.GetUnderlyingType(t);
            if (t2 == null)
            {
                // Not a nullable type.
                return false;
            }
            else
            {
                return scalarTypes.Contains(t2) || t2.IsEnum;
            }
        }

        /// <summary>
        /// Initialise a new ObjectDataReader that treats nullable types as nulls.
        /// </summary>
        /// <param name="collection">The collection of things to make a data reader over.</param>
        public ObjectDataReader(IEnumerable<T> collection)
            : this(collection, NullConversion.None)
        {
        }

        /// <summary>
        /// Initialise a new ObjectDataReader.
        /// </summary>
        /// <param name="collection">The collection of things to make a data reader over.</param>
        /// <param name="nullConversion">How to treat System.Nullable types.</param>
        public ObjectDataReader(IEnumerable<T> collection, NullConversion nullConversion)
        {
            collection.ThrowIfNull("collection");

            this.NullConversion = nullConversion;
            this.Enumerator = collection.GetEnumerator();

            // Done without a lock, so we risk running twice.
            // n.b. This is a scalar property, so the intention is that it is only
            // discovered once per instantiation of the app domain.
            if (scalarAttributes == null)
            {
                scalarAttributes = DiscoverScalarAttributes(typeof(T));
            }

            Attributes = scalarAttributes;
        }

        static List<Attribute> DiscoverScalarAttributes(Type thisType)
        {
            // Not a collection of entities, just an IEnumerable<String> or other scalar type.
            // So add just a single Attribute that returns the object itself.
            if (IsScalarType(thisType))
            {
                return new List<Attribute> { new Attribute("Value", thisType, t => t) };
            }

            // PropertyInfo and FieldInfo are subclasses of MemberInfo.
            // Find all the public scalar properties. These are PropertyInfo objects.
            List<MemberInfo> membersOfInterest = (from p in thisType.GetProperties()
                                                  where IsScalarType(p.PropertyType)
                                                  select p).Cast<MemberInfo>().ToList();

            // Add the public fields. These are FieldInfo objects.
            membersOfInterest.AddRange
                (
                (from f in thisType.GetFields()
                 where IsScalarType(f.FieldType)
                 select f).Cast<MemberInfo>()
                );

            var result = new List<Attribute>();
            foreach (var m in membersOfInterest)
            {
                PropertyInfo pi = m as PropertyInfo;
                if (pi != null)
                {
                    result.Add(new Attribute(pi));
                }
                else
                {
                    FieldInfo fi = m as FieldInfo;
                    if (fi != null)
                    {
                        result.Add(new Attribute(fi));
                    }
                    else
                    {
                        throw new InvalidOperationException("A new type of MemberInfo needs handling here.");
                    }
                }
            }

            return result;
        }

        const string schemaTableSchema = @"<?xml version=""1.0"" standalone=""yes""?>
<xs:schema id=""NewDataSet"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""NewDataSet"" msdata:IsDataSet=""true"" msdata:MainDataTable=""SchemaTable"" msdata:Locale="""">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""SchemaTable"" msdata:Locale="""" msdata:MinimumCapacity=""1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""ColumnOrdinal"" msdata:ReadOnly=""true"" type=""xs:int"" default=""0"" minOccurs=""0"" />
              <xs:element name=""ColumnSize"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""NumericPrecision"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""NumericScale"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""IsUnique"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsKey"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""BaseServerName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseCatalogName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseSchemaName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseTableName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""AllowDBNull"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""IsAliased"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsExpression"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsIdentity"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsAutoIncrement"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsRowVersion"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsHidden"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsLong"" msdata:ReadOnly=""true"" type=""xs:boolean"" default=""false"" minOccurs=""0"" />
              <xs:element name=""IsReadOnly"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderSpecificDataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataTypeName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionDatabase"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionOwningSchema"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""UdtAssemblyQualifiedName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""NonVersionedProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";

        /// <summary>
        /// Returns a System.Data.DataTable that describes the column metadata of the
        /// System.Data.IDataReader.
        /// </summary>
        /// <returns>A System.Data.DataTable that describes the column metadata.</returns>
        public DataTable GetSchemaTable()
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = System.Globalization.CultureInfo.CurrentCulture;
                using (var sr = new StringReader(schemaTableSchema))
                { 
                    ds.ReadXmlSchema(sr);
                    DataTable t = ds.Tables[0];

                    for (int i = 0; i < this.FieldCount; i++)
                    {
                        DataRow row = t.NewRow();
                        row["ColumnName"] = this.GetName(i);
                        row["ColumnOrdinal"] = i;

                        Type type = this.GetFieldType(i);
                        if (type.IsGenericType
                          && type.GetGenericTypeDefinition() == typeof(System.Nullable<int>).GetGenericTypeDefinition())
                        {
                            type = type.GetGenericArguments()[0];
                        }
                        row["DataType"] = this.GetFieldType(i);
                        row["DataTypeName"] = this.GetDataTypeName(i);
                        row["ColumnSize"] = -1;
                        t.Rows.Add(row);
                    }

                    return t;
                }
            }
        }

        /// <summary>
        /// Closes the data reader.
        /// </summary>
        public void Close()
        {
            closed = true;
        }

        /// <summary>
        /// Always returns 1.
        /// </summary>
        public int Depth
        {
            get { return 1; }
        }

        /// <summary>
        /// Returns true if the reader is closed.
        /// </summary>
        public bool IsClosed
        {
            get { return closed; }
        }

        /// <summary>
        /// Always returns false, the ObjectDataReader only supports one recordset at a time.
        /// </summary>
        /// <returns>False.</returns>
        public bool NextResult()
        {
            return false;
        }

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <returns>True if there are more rows; false otherwise.</returns>
        public bool Read()
        {
            if (IsClosed)
                throw new InvalidOperationException("You cannot called Read() on a data reader that is closed.");

            bool rv = Enumerator.MoveNext();
            if (rv)
            {
                current = Enumerator.Current;
            }
            else
            {
                // We have gone off the end.
                // Set this to null.
                current = default(T);
                Close();
            }

            return rv;
        }

        /// <summary>
        /// Always returns -1.
        /// </summary>
        public int RecordsAffected
        {
            get { return -1; }
        }

        /// <summary>
        /// Disposes the data reader.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <returns>Number of columns.</returns>
        public int FieldCount
        {
            get
            {
                return Attributes.Count;
            }
        }

        TField GetValue<TField>(int i)
        {
#if DEBUG
            if (current == null)
                throw new InvalidOperationException("'current' is null, this usually means you did not call Read() first or you have dropped off the end of the enumerable.");
            if (IsClosed)
                throw new InvalidOperationException("You called GetValue<T> on a closed data reader.");
#endif
            var attr = Attributes[i];
            TField val = (TField)attr.GetValue(current);
            return val;
        }

        /// <summary>
        /// Gets the boolean at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Boolean value.</returns>
        public bool GetBoolean(int i)
        {
            return GetValue<bool>(i);
        }

        /// <summary>
        /// Gets the Byte at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Byte value.</returns>
        public byte GetByte(int i)
        {
            return GetValue<byte>(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer
        /// as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var buf = GetValue<byte[]>(i);
            int bytes = Math.Min(length, buf.Length - (int)fieldOffset);
            Buffer.BlockCopy(buf, (int)fieldOffset, buffer, bufferoffset, bytes);
            return bytes;
        }

        /// <summary>
        /// Gets the char at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Char value.</returns>
        public char GetChar(int i)
        {
            return GetValue<char>(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer
        /// as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            string s = GetValue<string>(i);
            int chars = Math.Min(length, s.Length - (int)fieldoffset);
            s.CopyTo((int)fieldoffset, buffer, bufferoffset, chars);

            return chars;
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the data type information for the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The data type information for the specified field.</returns>
        public string GetDataTypeName(int i)
        {
            return Attributes[i].Type.Name;
        }

        /// <summary>
        /// Gets the DateTime at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>DateTime value.</returns>
        public DateTime GetDateTime(int i)
        {
            return GetValue<DateTime>(i);
        }

        /// <summary>
        /// Gets the Decimal at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Decimal value.</returns>
        public decimal GetDecimal(int i)
        {
            return GetValue<decimal>(i);
        }

        /// <summary>
        /// Gets the double at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Double value.</returns>
        public double GetDouble(int i)
        {
            return GetValue<double>(i);
        }

        /// <summary>
        /// Gets the System.Type information corresponding to the type of System.Object
        /// that would be returned from System.Data.IDataRecord.GetValue(System.Int32).
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The System.Type information corresponding to the type of System.Object that
        /// would be returned from System.Data.IDataRecord.GetValue(System.Int32).
        /// </returns>
        public Type GetFieldType(int i)
        {
            Type t = Attributes[i].Type;

            // If we will be converting int? to DBNull.Value instead of null,
            // then the type is int, not int?
            if (this.NullConversion == NullConversion.ToDBNull && IsNullableType(t))
                return Nullable.GetUnderlyingType(t);

            return t;
        }

        /// <summary>
        /// Gets the float at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Float value.</returns>
        public float GetFloat(int i)
        {
            return GetValue<float>(i);
        }

        /// <summary>
        /// Gets the Guid at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Guid value.</returns>
        public Guid GetGuid(int i)
        {
            return GetValue<Guid>(i);
        }

        /// <summary>
        /// Gets the Int16 (short) at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int16 (short) value.</returns>
        public short GetInt16(int i)
        {
            return GetValue<short>(i);
        }

        /// <summary>
        /// Gets the Int32 at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int32 value.</returns>
        public int GetInt32(int i)
        {
            return GetValue<int>(i);
        }

        /// <summary>
        /// Gets the Int64 at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Int64 value.</returns>
        public long GetInt64(int i)
        {
            return GetValue<long>(i);
        }

        /// <summary>
        /// Gets the name of the column at ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Name of the field.</returns>
        public string GetName(int i)
        {
            Attribute a = Attributes[i];
            return a.Name;
        }

        /// <summary>
        /// Gets the column ordinal for the column with the specified name.
        /// </summary>
        /// <param name="name">Name of the column.</param>
        /// <returns>Corresponding olumn ordinal.</returns>
        public int GetOrdinal(string name)
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                var a = Attributes[i];
                if (a.Name == name)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the string at column ordinal i.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>String value.</returns>
        public string GetString(int i)
        {
            return GetValue<string>(i);
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <param name="values">An array of System.Object to copy the attribute fields into.</param>
        /// <returns>The number of instances of System.Object in the array.</returns>
        public int GetValues(object[] values)
        {
            values.ThrowIfNull("values");
            if (values.Count() < Attributes.Count)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    "The values array has only {0} elements, which is not enough to hold the {1} elements in this reader.",
                    values.Count(), Attributes.Count
                    );
                throw new ArgumentException(msg, "values");
            }

            for (int i = 0; i < Attributes.Count; i++)
            {
                values[i] = GetValue(i);
            }
            return Attributes.Count;
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Value from column i.</returns>
        public object GetValue(int i)
        {
            object o = GetValue<object>(i);
            if (NullConversion == NullConversion.ToDBNull && o == null)
                return DBNull.Value;

            return o;
        }

        /// <summary>
        /// Return whether the specified field is null.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>True if the field is DbNull, false otherwise.</returns>
        public bool IsDBNull(int i)
        {
            object o = GetValue<object>(i);
            return (o == null);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <returns>Value from the column.</returns>
        public object this[string name]
        {
            get
            {
                int ordinal = GetOrdinal(name);
                return GetValue(ordinal);
            }
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Value from column i.</returns>
        public object this[int i]
        {
            get { return GetValue(i); }
        }
    }
}
