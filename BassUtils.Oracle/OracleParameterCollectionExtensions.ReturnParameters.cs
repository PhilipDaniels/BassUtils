﻿using System.Data;
using Dawn;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle
{
    /// <summary>
    /// Extensions for the <c>OracleParameterCollection</c> class.
    /// </summary>
    public static partial class OracleParameterCollectionExtensions
    {
        /// <summary>
        /// Adds a scalar-type return parameter to the parameter collection. Use this for functions that
        /// returns ints, strings etc.
        /// The parameter must be the first parameter added to the collection, an exception
        /// will be thrown if it isn't.
        /// </summary>
        /// <param name="parameterCollection">The collection to add the parameter to.</param>
        /// <param name="oracleDbType">The type of the return parameter.</param>
        /// <returns>The parameter that was added. Use this to get the value later.</returns>
        public static OracleParameter AddReturnParameter(this OracleParameterCollection parameterCollection, OracleDbType oracleDbType)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();

            if (parameterCollection.Count != 0)
                throw new ArgumentException("The parameter collection is not empty, a RETURN parameter must be added as the first parameter");

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = oracleDbType;
            parameterCollection.Add(prm);
            return prm;
        }

        /// <summary>
        /// Adds a UDT object type return parameter to the parameter collection. Use this for functions that
        /// return SQL-level UDTs, i.e. those created with the "CREATE TYPE xyz AS OBJECT" syntax
        /// at schema level.
        /// The parameter must be the first parameter added to the collection, an exception
        /// will be thrown if it isn't.
        /// </summary>
        /// <param name="parameterCollection">The collection to add the parameter to.</param>
        /// <param name="udtTypeName">The name of the UDT type. This will be a two-part name of the
        /// form "SCHEMA.OBJECTTYPENAME".</param>
        /// <returns>The parameter that was added. Use this to get the value later.</returns>
        public static OracleParameter AddUdtObjectReturnParameter(this OracleParameterCollection parameterCollection, string udtTypeName)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

            if (parameterCollection.Count != 0)
                throw new ArgumentException("The parameter collection is not empty, a RETURN parameter must be added as the first parameter");

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = OracleDbType.Object;
            prm.UdtTypeName = udtTypeName;
            parameterCollection.Add(prm);
            return prm;
        }

        /// <summary>
        /// Adds a UDT nested table type (aka array) return parameter to the parameter collection. Use this for functions that
        /// return SQL-level nested tables, i.e. those created with the "CREATE TYPE tblXyz AS TABLE OF objAbc" syntax
        /// at schema level.
        /// The parameter must be the first parameter added to the collection, an exception
        /// will be thrown if it isn't.
        /// </summary>
        /// <param name="parameterCollection">The collection to add the parameter to.</param>
        /// <param name="udtTypeName">The name of the UDT type. This will be a two-part name of the
        /// form "SCHEMA.TABLETYPENAME".</param>
        /// <returns>The parameter that was added. Use this to get the value later.</returns>
        public static OracleParameter AddUdtArrayReturnParameter(this OracleParameterCollection parameterCollection, string udtTypeName)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

            if (parameterCollection.Count != 0)
                throw new ArgumentException("The parameter collection is not empty, a RETURN parameter must be added as the first parameter");

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = OracleDbType.Array;
            prm.UdtTypeName = udtTypeName;
            parameterCollection.Add(prm);
            return prm;
        }
    }
}