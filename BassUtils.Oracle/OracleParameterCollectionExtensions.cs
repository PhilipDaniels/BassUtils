using System.Data;
using Dawn;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle;

/*
 * TODO
 * - AddUdtObject (how do we pass a null object?)
 * - AddUdtArray (passing a null array makes no sense, but how do we pass an array that contains a null object?)
 */

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
    /// Returns the length of every string in the collection.
    /// </summary>
    private static int[] GetStringLengths(IEnumerable<string> values)
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

    public static OracleParameter AddRefCursor(
        this OracleParameterCollection parameterCollection,
        string parameterName,
        ParameterDirection parameterDirection = ParameterDirection.Output
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.OracleDbType = OracleDbType.RefCursor;
        parameterCollection.Add(prm);
        return prm;
    }

}
