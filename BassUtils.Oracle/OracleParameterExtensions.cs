using Dawn;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle
{
    public static class OracleParameterExtensions
    {
        /// <summary>
        /// Returns the current value of the parameter as a data reader.
        /// Typically this is only safe to call after the containing command has been
        /// executed and the parameter type is <c>OracleDbType.RefCursor</c>.
        /// </summary>
        /// <param name="prm">The parameter.</param>
        /// <returns>Data reader.</returns>
        public static OracleDataReader GetDataReader(this OracleParameter prm)
        {
            Guard.Argument(prm, nameof(prm)).NotNull();
            return ((OracleRefCursor)prm.Value).GetDataReader();
        }
    }
}