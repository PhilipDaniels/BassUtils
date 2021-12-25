using Dawn;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.Oracle
{
    /// <summary>
    /// Extensions for the <c>OracleParameter</c> class.
    /// </summary>
    public static class OracleParameterExtensions
    {
        /// <summary>
        /// Returns the current value of the parameter as a data reader.
        /// Typically this is only safe to call after the containing command has been
        /// executed and the parameter type is <c>OracleDbType.RefCursor</c>.
        /// </summary>
        /// <param name="prm">The parameter.</param>
        /// <returns>Data reader.</returns>
        public static OracleDataReader GetValueAsDataReader(this OracleParameter prm)
        {
            Guard.Argument(prm, nameof(prm)).NotNull();

            return ((OracleRefCursor)prm.Value).GetDataReader();
        }

        /// <summary>
        /// Attempts to retrieve the <c>Value</c> property as a list of strings.
        /// </summary>
        /// <param name="prm">The OracleParameter object.</param>
        /// <returns>List of strings.</returns>
        /// <exception cref="ArgumentException">Value property could not be expressed as a list of strings.</exception>
        public static List<string> GetValueAsStringList(this OracleParameter prm)
        {
            Guard.Argument(prm, nameof(prm)).NotNull();

            if (prm.Value == null)
                return new List<string>();

            string[] v1 = prm.Value as string[];
            if (v1 != null)
                return v1.ToList();

            OracleString[] v2 = prm.Value as OracleString[];
            if (v2 != null)
                return v2.Select(s => s.IsNull ? null : s.Value).ToList();

            throw new ArgumentException($"Cannot map prm.Value of type {prm.Value.GetType().Name} to string list");
        }

        /// <summary>
        /// Attempts to retrieve the <c>Value</c> property as a list of nullable decimals.
        /// </summary>
        /// <param name="prm">The OracleParameter object.</param>
        /// <returns>List of nullable decimals.</returns>
        /// <exception cref="ArgumentException">Value property could not be expressed as a list of nullable decimals.</exception>
        public static List<decimal?> GetValueAsNullableDecimalList(this OracleParameter prm)
        {
            Guard.Argument(prm, nameof(prm)).NotNull();

            if (prm.Value == null)
                return new List<decimal?>();

            OracleDecimal[] v2 = prm.Value as OracleDecimal[];
            if (v2 != null)
                return v2.Select(s => s.IsNull ? (decimal?)null : s.Value).ToList();

            decimal?[] v1 = prm.Value as decimal?[];
            if (v1 != null)
                return v1.ToList();

            throw new ArgumentException($"Cannot map prm.Value of type {prm.Value.GetType().Name} to List<decimal?>");
        }

        /// <summary>
        /// Attempts to retrieve the <c>Value</c> property as a list of decimals.
        /// </summary>
        /// <remarks>
        /// This method will throw if the list contains any NULLs.</remarks>
        /// <param name="prm">The OracleParameter object.</param>
        /// <returns>List of decimals.</returns>
        /// <exception cref="ArgumentException">Value property could not be expressed as a list of decimals.</exception>
        public static List<decimal> GetValueAsDecimalList(this OracleParameter prm)
        {
            Guard.Argument(prm, nameof(prm)).NotNull();

            if (prm.Value == null)
                return new List<decimal>();

            OracleDecimal[] v2 = prm.Value as OracleDecimal[];
            if (v2 != null)
                return v2.Select(s => s.Value).ToList();

            decimal[] v1 = prm.Value as decimal[];
            if (v1 != null)
                return v1.ToList();

            throw new ArgumentException($"Cannot map prm.Value of type {prm.Value.GetType().Name} to List<decimal?>");
        }
    }
}