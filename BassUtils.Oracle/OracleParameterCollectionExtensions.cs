using System.Data;
using Oracle.ManagedDataAccess.Client;

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
    /// Returns the length of every string in the collection.
    /// </summary>
    private static int[] GetStringLengths(IEnumerable<string> values)
    {
        return values.Select(d => d.Length).ToArray();
    }

    private static OracleParameter MakeReturnParameter(OracleDbType oracleDbType)
    {
        return new OracleParameter
        {
            // Name does not matter for RETURN parameters, can be anything.
            ParameterName = "RETURN_VALUE",
            Direction = ParameterDirection.ReturnValue,
            OracleDbType = oracleDbType
        };
    }

    private static void CheckCollectionEmpty(OracleParameterCollection parameterCollection)
    {
        if (parameterCollection.Count != 0)
            throw new ArgumentException("The parameter collection is not empty, a RETURN parameter must be added as the first parameter");
    }
}
