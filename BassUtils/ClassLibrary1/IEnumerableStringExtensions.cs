using System.Collections.Generic;
using System.Linq;
using Dawn;

namespace ClassLibrary1
{
    /// <summary>
    /// Extensions to <c>IEnumerable&lt;string&gt;.</c>
    /// </summary>
    public static class IEnumerableStringExtensions
    {
        /// <summary>
        /// Applies the <c>Trim()</c> function to all the strings in the sequence, individual strings can be null,
        /// no error will be raised if that is true.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>Set of trimmed strings.</returns>
        public static IEnumerable<string> TrimAll(this IEnumerable<string> values)
        {
            Guard.Argument(values, nameof(values)).NotNull();

            return values.Select(s => s == null ? null : s.Trim());
        }
    }
}
