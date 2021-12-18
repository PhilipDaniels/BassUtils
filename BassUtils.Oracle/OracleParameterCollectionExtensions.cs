using System.Data;
using Dawn;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle;

/// <summary>
/// Extensions for the <c>OracleParameterCollection</c> class.
/// </summary>
public static partial class OracleParameterCollectionExtensions
{
    /// <summary>
    /// Returns the length of every string in the array.
    /// </summary>
    private static int[] GetStringLengths(string[] values)
    {
        return values.Select(d => d.Length).ToArray();
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to a single Oracle UDT,
    /// i.e. one defined at schema-level using the "CREATE TYPE objXyz AS OBJECT" syntax.
    /// </summary>
    /// <typeparam name="T">The type of the parameter value.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="udtTypeName">The name of the UDT type. This will be a two-part name of the
    /// form "SCHEMA.OBJECTTYPENAME".</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddUdtObjectParameter<T>
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        string udtTypeName,
        T value,
        ParameterDirection parameterDirection = ParameterDirection.Input
        )
    where T : IOracleCustomType, INullable
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.UdtTypeName = udtTypeName;
        prm.OracleDbType = OracleDbType.Object;
        prm.Value = value;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an array of Oracle UDTs,
    /// i.e. one defined at schema-level using the "CREATE TYPE tblXYZ AS TABLE OF objXYZ" syntax.
    /// </summary>
    /// <typeparam name="T">The type of the parameter value.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="udtTypeName">The name of the UDT type. This will be a two-part name of the
    /// form "SCHEMA.TABLETYPENAME".</param>
    /// <param name="value"></param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddUdtArrayParameter<T>(
        this OracleParameterCollection parameterCollection,
        string parameterName,
        string udtTypeName,
        T value,
        ParameterDirection parameterDirection = ParameterDirection.Input
        )
         where T : IOracleCustomType, INullable
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.UdtTypeName = udtTypeName;
        prm.OracleDbType = OracleDbType.Array;
        prm.Value = value;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array,
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the values collection.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter<T>(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      params T[] values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Value = values;
        prm.Size = values.Length;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array,
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the values collection.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter<T>(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      IEnumerable<T> values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        return AddAssociativeArrayParameter(parameterCollection, parameterName, parameterDirection, values.ToArray());
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array of strings.
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF VARCHAR2(n) INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      params string[] values
  )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Value = values;
        prm.Size = values.Length;
        // ArrayBindSize is the length of each string.
        prm.ArrayBindSize = GetStringLengths(values);
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array of strings.
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF VARCHAR2(n) INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      IEnumerable<string> values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        return AddAssociativeArrayParameter(parameterCollection, parameterName, parameterDirection, values.ToArray());
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to array binding.
    /// This does not require any server-side definitions, it works by sending a single
    /// SQL statement and multiple sets of data in one network call.
    /// See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E
    /// Section 3.8.3.5 Array Binding.
    /// <b>WARNING: Caller must set the ArrayBindCount property on the containing command to the length of the <paramref name="values"/> array,
    /// which means all arrays passed to the statement must be the same length.</b>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="oracleDbType">The corresponding Oracle type of the values.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter<T>(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      OracleDbType oracleDbType,
      ParameterDirection parameterDirection,
      params T[] values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.OracleDbType = oracleDbType;
        prm.Value = values;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to array binding.
    /// This does not require any server-side definitions, it works by sending a single
    /// SQL statement and multiple sets of data in one network call.
    /// See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E
    /// Section 3.8.3.5 Array Binding.
    /// <b>WARNING: Caller must set the ArrayBindCount property on the containing command to the length of the <paramref name="values"/> array,
    /// which means all arrays passed to the statement must be the same length.</b>
    /// </summary>
    /// <typeparam name="T">The type of elements in the values.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="oracleDbType">The corresponding Oracle type of the values.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter<T>(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      OracleDbType oracleDbType,
      ParameterDirection parameterDirection,
      IEnumerable<T> values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        return AddArrayParameter(parameterCollection, parameterName, oracleDbType, parameterDirection, values.ToArray());
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to array binding.
    /// This does not require any server-side definitions, it works by sending a single
    /// SQL statement and multiple sets of data in one network call.
    /// See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E
    /// Section 3.8.3.5 Array Binding.
    /// <b>WARNING: Caller must set the ArrayBindCount property on the containing command to the length of the <paramref name="values"/> array,
    /// which means all arrays passed to the statement must be the same length.</b>
    /// </summary>
    /// <remarks>
    /// This string overload sets the OracleDbType to Varchar2 and automatically calculates
    /// the length of each string in the array.
    /// </remarks>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      params string[] values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.OracleDbType = OracleDbType.Varchar2;
        prm.Value = values;
        // ArrayBindSize is the length of each string.
        prm.ArrayBindSize = GetStringLengths(values);
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to array binding.
    /// This does not require any server-side definitions, it works by sending a single
    /// SQL statement and multiple sets of data in one network call.
    /// See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E
    /// Section 3.8.3.5 Array Binding.
    /// <b>WARNING: Caller must set the ArrayBindCount property on the containing command to the length of the <paramref name="values"/> array,
    /// which means all arrays passed to the statement must be the same length.</b>
    /// </summary>
    /// <remarks>
    /// This string overload sets the OracleDbType to Varchar2 and automatically calculates
    /// the length of each string in the array.
    /// </remarks>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter(
      this OracleParameterCollection parameterCollection,
      string parameterName,
      ParameterDirection parameterDirection,
      IEnumerable<string> values
      )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        return AddArrayParameter(parameterCollection, parameterName, parameterDirection, values.ToArray());
    }
}
