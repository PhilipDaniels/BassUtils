using System.Data;
using Dawn;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle;

/// <summary>
/// Extensions for the <c>OracleParameterCollection</c> class.
/// </summary>
public static partial class OracleParameterCollectionExtensions
{
    /// <summary>
    /// Adds an OracleParameter that corresponds to array binding.
    /// This does not require any server-side definitions, it works by sending a single
    /// SQL statement and multiple sets of data in one network call.
    /// See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E
    /// Section 3.8.3.5 Array Binding.
    /// <b>WARNING: Caller must set the ArrayBindCount property on the containing command to the length of the <paramref name="values"/> array,
    /// which means all arrays passed to the statement must be the same length.</b>
    /// </summary>
    /// <typeparam name="T">The type of elements in the values collection.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="oracleDbType">The corresponding Oracle type of the values.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter<T>
        (
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
    /// <remarks>
    /// This string overload sets the OracleDbType to Varchar2 and automatically calculates
    /// the length of each string in the array.
    /// </remarks>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values in the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddArrayParameter
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        ParameterDirection parameterDirection,
        IEnumerable<string> values
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
}
