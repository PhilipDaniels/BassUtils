using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>System.Reflection.MemberInfo</code> class.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Returns all the attributes of a member or type.
        /// </summary>
        /// <typeparam name="T">The type of the attribute.</typeparam>
        /// <param name="member">The member to get attributes of.</param>
        /// <returns>List of attributes. Can be empty. Will not be null.</returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo member)
        {
            member.ThrowIfNull("member");

            var attributes = member.GetCustomAttributes(typeof(T), true);
            return attributes.Cast<T>();
        }
    }
}
