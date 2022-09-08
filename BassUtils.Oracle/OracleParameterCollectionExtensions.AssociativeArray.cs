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
    /// Creates and adds to the collection a new IN or IN OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is for passing arrays of other types not handled by
    /// specific overloads. The type of element is normally guessed by ODP.Net,
    /// and can be left null, but sometimes you might need to specify it
    /// explicitly.
    /// </summary>
    /// <remarks>
    /// For OUT parameters use the appropriate overload of <c>AddOutputAssociativeArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="values">Values to pass to Oracle.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <param name="oracleDbType">Oracle type - usually null to allow ODP.Net to
    /// infer the type. If ODP.Net infers incorrectly, try specifying it explicitly.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddAssociativeArray<T>
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       IEnumerable<T> values,
       ParameterDirection parameterDirection = ParameterDirection.Input,
       OracleDbType? oracleDbType = null
       )
    {
        return InnerAddAssociativeArray(parameterCollection, parameterName, values, parameterDirection, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new IN or IN OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is specifically for passsing arrays of floats.
    /// </summary>
    /// <remarks>
    /// For OUT parameters use the appropriate overload of <c>AddOutputAssociativeArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="values">Values to pass to Oracle.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <param name="oracleDbType">Oracle type - usually BinaryFloat.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddAssociativeArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       IEnumerable<float> values,
       ParameterDirection parameterDirection = ParameterDirection.Input,
       OracleDbType oracleDbType = OracleDbType.BinaryFloat
       )
    {
        return InnerAddAssociativeArray(parameterCollection, parameterName, values, parameterDirection, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new IN or IN OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is specifically for passing arrays of doubles.
    /// </summary>
    /// <remarks>
    /// For OUT parameters use the appropriate overload of <c>AddOutputAssociativeArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="values">Values to pass to Oracle.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <param name="oracleDbType">Oracle type - usually BinaryDouble.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddAssociativeArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       IEnumerable<double> values,
       ParameterDirection parameterDirection = ParameterDirection.Input,
       OracleDbType oracleDbType = OracleDbType.BinaryDouble
       )
    {
        return InnerAddAssociativeArray(parameterCollection, parameterName, values, parameterDirection, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new IN or IN OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is specifically for passing arrays of DateTimes.
    /// </summary>
    /// <remarks>
    /// For OUT parameters use the appropriate overload of <c>AddOutputAssociativeArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="values">Values to pass to Oracle.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <param name="oracleDbType">Oracle type - usually Date.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddAssociativeArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       IEnumerable<DateTime> values,
       ParameterDirection parameterDirection = ParameterDirection.Input,
       OracleDbType oracleDbType = OracleDbType.Date
       )
    {
        return InnerAddAssociativeArray(parameterCollection, parameterName, values, parameterDirection, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new IN or IN OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is specifically for passing arrays of strings.
    /// </summary>
    /// <remarks>
    /// For OUT parameters use the appropriate overload of <c>AddOutputAssocStringArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="values">Values to pass to Oracle.</param>
    /// <param name="parameterDirection">Parameter direction.</param>
    /// <param name="oracleDbType">Oracle type - usually Varchar2 or NVarchar2.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddAssociativeArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       IEnumerable<string> values,
       ParameterDirection parameterDirection = ParameterDirection.Input,
       OracleDbType oracleDbType = OracleDbType.Varchar2
       )
    {
        var prm = InnerAddAssociativeArray(parameterCollection, parameterName, values, parameterDirection, oracleDbType);
        prm.ArrayBindSize = GetStringLengths(values);
        return prm;
    }

    /// <summary>
    /// Creates and adds to the collection a new OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// </summary>
    /// <remarks>
    /// For IN or IN OUT parameters use the appropriate overload of <c>AddAssociativeArray</c>.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="oracleDbType">Oracle type of elements in the array.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddOutputAssociativeArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       int arraySize,
       OracleDbType oracleDbType
       )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = ParameterDirection.Output;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Size = arraySize;
        prm.OracleDbType = oracleDbType;
        parameterCollection.Add(prm);

        return prm;
    }

    /// <summary>
    /// Creates and adds to the collection a new OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is for retrieving arrays of strings.
    /// </summary>
    /// <remarks>
    /// For IN or IN OUT parameters use the overload of <c>AddAssociativeArray</c> that takes
    /// a collection of strings.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="maxStringLength">The maximum length of each string in the array.</param>
    /// <param name="oracleDbType">Oracle type - usually Varchar2 or NVarchar2.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddOutputAssociativeStringArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       int arraySize,
       int maxStringLength,
       OracleDbType oracleDbType = OracleDbType.Varchar2
       )
    {
        var arrayBindSizes = Enumerable.Repeat(maxStringLength, arraySize).ToArray();
        return AddOutputAssociativeStringArray(parameterCollection, parameterName, arraySize, arrayBindSizes, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new OUT parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is for retrieving arrays of strings, but
    /// the overload that takes a <c>maxStringLength</c> is easier to use.
    /// </summary>
    /// <remarks>
    /// For IN or IN OUT parameters use the overload of <c>AddAssociativeArray</c> that takes
    /// a collection of strings.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="arrayBindSizes">An array of bind sizes, that is an array of ints
    /// where each element is the maximum expected size of a string in the array.</param>
    /// <param name="oracleDbType">Oracle type - usually Varchar2 or NVarchar2.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddOutputAssociativeStringArray
       (
       this OracleParameterCollection parameterCollection,
       string parameterName,
       int arraySize,
       int[] arrayBindSizes,
       OracleDbType oracleDbType = OracleDbType.Varchar2
       )
    {
        var prm = AddOutputAssociativeArray(parameterCollection, parameterName, arraySize, oracleDbType);
        prm.ArrayBindSize = arrayBindSizes;
        return prm;
    }

    /// <summary>
    /// Creates and adds to the collection a new RETURN parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// </summary>
    /// <remarks>
    /// This method is for retrieving the value returned by an Oracle FUNCTION.
    /// Hold on to the parameter object returned and use it to access the <c>Value</c> property.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="oracleDbType">Oracle type of elements in the array.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddReturnAssociativeArray
        (
        this OracleParameterCollection parameterCollection,
        int arraySize,
        OracleDbType oracleDbType
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        CheckCollectionEmpty(parameterCollection);

        var prm = MakeReturnParameter(oracleDbType);
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Size = arraySize;
        parameterCollection.Add(prm);
        return prm;
    }

    /// <summary>
    /// Creates and adds to the collection a new RETURN parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is for retrieving arrays of strings.
    /// </summary>
    /// <remarks>
    /// This method is for retrieving the value returned by an Oracle FUNCTION.
    /// Hold on to the parameter object returned and use it to access the <c>Value</c> property.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="maxStringLength">The maximum length of an individual string in
    /// the returned array.</param>
    /// <param name="oracleDbType">Oracle type of elements in the array.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddReturnAssociativeStringArray
        (
        this OracleParameterCollection parameterCollection,
        int arraySize,
        int maxStringLength,
        OracleDbType oracleDbType = OracleDbType.Varchar2
        )
    {
        var arrayBindSizes = Enumerable.Repeat(maxStringLength, arraySize).ToArray();
        return AddReturnAssociativeStringArray(parameterCollection, arraySize, arrayBindSizes, oracleDbType);
    }

    /// <summary>
    /// Creates and adds to the collection a new RETURN parameter of type
    /// associative array, that is one defined at package level with the
    /// "TYPE arrXYZ IS TABLE OF abc INDEX BY ..." syntax.
    /// This overload is for retrieving arrays of strings, but
    /// the overload that takes a <c>maxStringLength</c> is easier to use.
    /// </summary>
    /// <remarks>
    /// This method is for retrieving the value returned by an Oracle FUNCTION.
    /// Hold on to the parameter object returned and use it to access the <c>Value</c> property.
    /// </remarks>
    /// <param name="parameterCollection">The parameter collection.</param>
    /// <param name="arraySize">Expected size of the array - pass a number at least as
    /// large as what you expect to get back. It can be larger.</param>
    /// <param name="arrayBindSizes">An array of bind sizes, that is an array of ints
    /// where each element is the maximum expected size of a string in the array.</param>
    /// <param name="oracleDbType">Oracle type of elements in the array.</param>
    /// <returns>The new parameter object.</returns>
    public static OracleParameter AddReturnAssociativeStringArray
        (
        this OracleParameterCollection parameterCollection,
        int arraySize,
        int[] arrayBindSizes,
        OracleDbType oracleDbType = OracleDbType.Varchar2
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        CheckCollectionEmpty(parameterCollection);

        var prm = MakeReturnParameter(oracleDbType);
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Size = arraySize;
        prm.ArrayBindSize = arrayBindSizes;
        parameterCollection.Add(prm);
        return prm;
    }

    private static OracleParameter InnerAddAssociativeArray<T>
        (
        this OracleParameterCollection parameterCollection,
        string parameterName,
        IEnumerable<T> values,
        ParameterDirection parameterDirection = ParameterDirection.Input,
        OracleDbType? oracleDbType = null
        )
    {
        Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
        Guard.Argument(parameterName, nameof(parameterName)).NotNull().NotWhiteSpace();
        Guard.Argument(values, nameof(values)).NotNull();

        var prm = new OracleParameter();
        prm.ParameterName = parameterName;
        prm.Direction = parameterDirection;
        prm.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
        prm.Value = values.ToArray();
        prm.Size = values.Count();
        if (oracleDbType != null)
            prm.OracleDbType = oracleDbType.Value;
        parameterCollection.Add(prm);

        return prm;
    }
}
