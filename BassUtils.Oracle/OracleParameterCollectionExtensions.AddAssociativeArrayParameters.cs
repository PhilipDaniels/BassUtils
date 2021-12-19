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
    /// Adds an OracleParameter that corresponds to an associative array,
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <remarks>
    /// If you wish to specify a null array, use the overload that takes a size and pass null for the values
    /// and set the size to the size you expect.</remarks>
    /// <typeparam name="T">The type of the elements in the values collection.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter<T>
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        ParameterDirection parameterDirection,
        IEnumerable<T> values
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        return AddAssociativeArrayParameter
            (
            parameterCollection,
            parameterName,
            parameterDirection,
            values,
            values.Count()
            );
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array,
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <remarks>
    /// This overload is useful when you wish to pass in a null array.</remarks>
    /// <typeparam name="T">The type of the elements in the values collection.</typeparam>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <param name="size">The size of the array.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter<T>
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        ParameterDirection parameterDirection,
        IEnumerable<T> values,
        int size
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Value = values;
        prm.Size = size;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array of strings.
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF VARCHAR2(n) INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level. This overload automatically calculates the
    /// lengths of the strings that are being passed (it assumes there are no null strings with the
    /// <paramref name="values"/> collection).
    /// </summary>
    /// <remarks>
    /// If you wish to specify a null array, use the overload that takes a size and pass null for the values
    /// and set the size to the size you expect.</remarks>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter
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

        return AddAssociativeArrayParameter
            (
            parameterCollection,
            parameterName,
            parameterDirection,
            values,
            values.Count(),
            GetStringLengths(values)
            );
    }

    /// <summary>
    /// Adds an OracleParameter that corresponds to an associative array of strings.
    /// i.e. one defined at PACKAGE level using the "TYPE arrXYZ IS TABLE OF VARCHAR2(n) INDEX BY ..." syntax.
    /// Such arrays may be used at package or schema level.
    /// </summary>
    /// <remarks>
    /// This overload is useful when you wish to pass in a null array.</remarks>
    /// <param name="parameterCollection">The collection to add the parameter to.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="parameterDirection">The direction of the parameter.</param>
    /// <param name="values">The values to set as the parameter value.</param>
    /// <param name="size">The size of the array.</param>
    /// <param name="arrayBindSize">The size of each string within the array. If you pass a null
    /// array, set this to the sizes of the expected results, e.g. 20 for a VARCHAR2(20) column.</param>
    /// <returns>The parameter that was added.</returns>
    public static OracleParameter AddAssociativeArrayParameter
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        ParameterDirection parameterDirection,
        IEnumerable<string> values,
        int size,
        int[] arrayBindSize
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Value = values;
        prm.Size = size;
        // ArrayBindSize is the length of each string.
        prm.ArrayBindSize = arrayBindSize;

        parameterCollection.Add(prm);
        return prm;
    }
}
