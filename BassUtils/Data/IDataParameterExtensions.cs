using System;
using System.Data;
using Dawn;

namespace BassUtils.Data
{
    /// <summary>
    /// Extension methods to IDataParamter to make setting nullable parameters easier.
    /// </summary>
    public static class IDataParameterExtensions
    {
        /// <summary>
        /// Sets the <code>.Value</code> property of <paramref name="parameter"/>.
        /// If the string is null, then the <code>.Value</code>
        /// is set to DBNull.Value, otherwise it is set to the value. No trimming is done.
        /// </summary>
        /// <param name="parameter">The parameter to have its value set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue(this IDataParameter parameter, string value)
        {
            Guard.Argument(parameter, nameof(parameter)).NotNull();

            if (value == null)
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
            Guard.Argument(parameter, nameof(parameter)).NotNull();

            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }
    }
}