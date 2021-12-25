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
    /// Adds a parameter of RefCursor type. This can be used to retrieve recordsets from
    /// Oracle, typically the procedure parameter would be declared as 'pCursor OUT SYS_REFCURSOR'.
    /// </summary>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddRefCursor
        (
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

    /// <summary>
    /// Adds a RETURN parameter of RefCursor type. This can be used to retrieve recordsets from
    /// Oracle, typically the function would be declared as 'RETURN SYS_REFCURSOR'.
    /// </summary>
    /// <remarks>
    /// This parameter must be the first one added to the collection, an exception will
    /// be thrown if it isn't.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddReturnRefCursor(this OracleParameterCollection parameterCollection)
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        CheckCollectionEmpty(parameterCollection);

        return AddRefCursor(parameterCollection, "RETURN_REF_CURSOR", ParameterDirection.ReturnValue);
    }
}
