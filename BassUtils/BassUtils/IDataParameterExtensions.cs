using System;
using System.Data;

namespace BassUtils
{
    /// <summary>
    /// Extension methods to IDataParamter to make setting nullable parameters easier.
    /// </summary>
    public static class IDataParameterExtensions
    {
        /// <summary>
        /// Sets the <code>.Value</code> property of <paramref name="parameter"/>.
        /// If the string is null, or Trim()s to empty string then the <code>.Value</code>
        /// is set to DBNull.Value, otherwise it is set to the value of the text
        /// field, with any *'s replaced with %'s.
        /// </summary>
        /// <param name="parameter">The parameter to have its value set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue(this IDataParameter parameter, string value)
        {
            parameter.ThrowIfNull("parameter");

            if (String.IsNullOrWhiteSpace(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value.Trim();
        }

        /// <summary>
        /// Sets the <code>.Value</code> property of <paramref name="parameter"/>.
        /// If the value is null, then the <code>.Value</code>
        /// is set to DBNull.Value, otherwise it is set to <paramref name="value"/>.
        /// </summary>
        /// <param name="parameter">The parameter to have its value set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue<T>(this IDataParameter parameter, T? value)
            where T : struct
        {
            parameter.ThrowIfNull("parameter");

            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }
    }
}
