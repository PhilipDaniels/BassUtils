using System.Data;
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
        public static OracleParameter AddReturn(this OracleParameterCollection parameterCollection, OracleDbType oracleDbType)
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
    }
}