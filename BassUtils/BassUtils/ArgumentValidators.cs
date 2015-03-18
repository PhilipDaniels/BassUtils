using System;
using System.Globalization;
using System.IO;

namespace BassUtils
{
    /// <summary>
    /// Provides utility methods for validating arguments to methods.
    /// </summary>
    public static class ArgumentValidators
    {
        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string parameterName)
        {
            return parameter.ThrowIfNull(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string parameterName)
        {
            return parameter.ThrowIfNullOrEmpty(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);
            if (parameter.Length == 0)
                throw new ArgumentException(message, parameterName);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string parameterName)
        {
            return parameter.ThrowIfNullOrWhiteSpace(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);
            if (parameter.Trim().Length == 0)
                throw new ArgumentException(message, parameterName);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThan(comparisonValue, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName, string message)
            where T : IComparable<T>
        {
            parameter.ThrowIfNull(parameterName, message);

            if (parameter.CompareTo(comparisonValue) < 0)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_LessThan,
                    comparisonValue
                    );

                if (message != null)
                    msg = message + " " + msg;

                throw new ArgumentOutOfRangeException(parameterName, parameter, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than 
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThanOrEqualTo(comparisonValue, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName, string message)
            where T : IComparable<T>
        {
            parameter.ThrowIfNull(parameterName, message);

            if (parameter.CompareTo(comparisonValue) <= 0)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_LessThanOrEqualTo,
                    comparisonValue
                    );

                if (message != null)
                    msg = message + " " + msg;

                throw new ArgumentOutOfRangeException(parameterName, parameter, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThan(comparisonValue, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName, string message)
            where T : IComparable<T>
        {
            parameter.ThrowIfNull(parameterName, message);

            if (parameter.CompareTo(comparisonValue) > 0)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_MoreThan,
                    comparisonValue
                    );

                if (message != null)
                    msg = message + " " + msg;

                throw new ArgumentOutOfRangeException(parameterName, parameter, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThanOrEqualTo(comparisonValue, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T comparisonValue, string parameterName, string message)
        where T : IComparable<T>
        {
            parameter.ThrowIfNull(parameterName, message);

            if (parameter.CompareTo(comparisonValue) >= 0)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_MoreThanOrEqualTo,
                    comparisonValue
                    );

                if (message != null)
                    msg = message + " " + msg;

                throw new ArgumentOutOfRangeException(parameterName, parameter, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws a <code>ArgumentNullException</code> if <paramref name="path"/> is null.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="path"/> is whitespace.
        /// Throws a <code>FileNotFoundException</code> if the file <paramref name="path"/> does not exist.
        /// </summary>
        /// <param name="path">Path of the directory.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfFileDoesNotExist([ValidatedNotNull] this string path, string parameterName)
        {
            path.ThrowIfNullOrWhiteSpace(parameterName, Properties.Resources.ArgVal_PathMustBeSpecified);

            if (!File.Exists(path))
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_FileDoesNotExist,
                    path, parameterName
                    );

                throw new FileNotFoundException(msg, path);
            }

            return path;
        }

        /// <summary>
        /// Throws a <code>ArgumentNullException</code> if <paramref name="path"/> is null.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="path"/> is whitespace.
        /// Throws a <code>DirectoryNotFoundException</code> if the directory <paramref name="path"/> does not exist.
        /// </summary>
        /// <param name="path">Path of the directory.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfDirectoryDoesNotExist([ValidatedNotNull] this string path, string parameterName)
        {
            path.ThrowIfNullOrWhiteSpace(parameterName, Properties.Resources.ArgVal_PathMustBeSpecified);

            if (!Directory.Exists(path))
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_DirectoryDoesNotExist,
                    path, parameterName
                    );

                throw new DirectoryNotFoundException(msg);
            }

            return path;
        }

        /// <summary>
        /// Throws a <code>ArgumentException</code> if <typeparamref name="T"/> is not an enumerated type.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="enumerand"/> is not a valid value within <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="enumerand">The value of the enumeration.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="enumerand"/> if no exception is thrown.</returns>
        public static T ThrowIfInvalidEnumerand<T>([ValidatedNotNull] this T enumerand, string parameterName)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_NotAnEnumeratedType,
                    enumType.FullName
                    );

                throw new ArgumentException(msg);
            }

            if (!Enum.IsDefined(enumType, enumerand))
            {
                string msg = String.Format
                    (
                    CultureInfo.InvariantCulture,
                    Properties.Resources.ArgVal_NotValidEnumeratedValue,
                    enumType.FullName
                    );

                throw new ArgumentOutOfRangeException(parameterName, enumerand, msg);
            }

            return enumerand;
        }
    }
}
