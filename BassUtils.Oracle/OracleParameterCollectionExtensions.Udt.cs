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
    /// Adds an OracleParameter that corresponds to a single Oracle UDT,
    /// i.e. one defined at schema-level using the "CREATE TYPE objXyz AS OBJECT" syntax.
    /// </summary>
    /// <typeparam name="T">The type of the parameter value.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="udtTypeName">The name of the UDT type. This will be a two-part name of the
    /// form "SCHEMA.OBJECTTYPENAME".</param>
    /// <param name="value">The value of the parameter. Must be a special <c>IOracleCustomType</c> implementation.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddUdtObject<T>
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
    /// <param name="value">The value of the parameter. Must be a special <c>IOracleCustomType</c> implementation
    /// that is a collection type.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddUdtArray<T>(
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
    public static OracleParameter AddReturnUdtObject(this OracleParameterCollection parameterCollection, string udtTypeName)
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();
        CheckCollectionEmpty(parameterCollection);

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
    public static OracleParameter AddReturnUdtArray(this OracleParameterCollection parameterCollection, string udtTypeName)
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

        CheckCollectionEmpty(parameterCollection);

        var prm = new OracleParameter();
        prm.ParameterName = "RETURN_VALUE";
        prm.Direction = ParameterDirection.ReturnValue;
        prm.OracleDbType = OracleDbType.Array;
        prm.UdtTypeName = udtTypeName;
        parameterCollection.Add(prm);
        return prm;
    }
}
