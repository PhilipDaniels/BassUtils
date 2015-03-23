using System;
using System.Data;
using System.Data.SqlClient;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>System.Data.SqlClient.SqlParameterCollection</code> class.
    /// </summary>
    public static class SqlParameterCollectionExtensions
    {
        /// <summary>
        /// Iterates over all the parameters in the collection and sets their value to <c>DBNull.Value</c>
        /// if they are (C#) <c>null</c>. This is a handy alternative to setting them as you go along, just
        /// call this method before you execute the command to ensure everything is set to <c>DBNull.Value</c>
        /// where it needs to be.
        /// </summary>
        /// <param name="parameters">The set of Sql Parameters.</param>
        public static void NullifyAllParameters(this SqlParameterCollection parameters)
        {
            parameters.ThrowIfNull("parameters");

            foreach (SqlParameter param in parameters)
            {
                if (param.Value == null)
                {
                    param.Value = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Adds a parameter to the collection, inserting DbNull if the parameter is null.
        /// </summary>
        /// <typeparam name="T">The type of object to add.</typeparam>
        /// <param name="parameters">The parameters collection to add the new parameter to.</param>
        /// <param name="parameterName">The (SQL name) of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The new SqlParameter object that was just added to the collection.</returns>
        public static SqlParameter AddWithNullableValue<T>
            (
            this SqlParameterCollection parameters,
            string parameterName,
            T? value
            )
        where T : struct
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        /// <summary>
        /// Add a string of type 'varchar' to the collection.
        /// </summary>
        /// <param name="parameters">The parameters collection to add the new parameter to.</param>
        /// <param name="parameterName">The (SQL name) of the parameter.</param>
        /// <param name="size">The size of the string parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The new SqlParameter object that was just added to the collection.</returns>
        public static SqlParameter AddString
            (
            this SqlParameterCollection parameters,
            string parameterName,
            int size,
            string value
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter(parameterName, SqlDbType.VarChar, size);
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        /// <summary>
        /// When adding an nvarchar, size (for this method) is just the length of the column!
        /// </summary>
        /// <param name="parameters">The collection.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="size">The length of the column, as you typed it in SSMS.</param>
        /// <param name="value">The string value.</param>
        /// <returns>The new SqlParameter object that was just added to the collection.</returns>
        public static SqlParameter AddNVString
            (
            this SqlParameterCollection parameters,
            string parameterName,
            int size,
            string value
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            if (size != -1)
                size *= 2;

            var p = new SqlParameter(parameterName, SqlDbType.NVarChar);
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        /// <summary>
        /// Adds an output parameter named 'Id' of type <code>SqlDbType.Int</code> to the parameter collection.
        /// </summary>
        /// <param name="parameters">The collection.</param>
        /// <returns>The new SqlParameter object that was just added to the collection.</returns>
        public static SqlParameter AddOutputId(this SqlParameterCollection parameters)
        {
            return AddOutputId(parameters, "Id", SqlDbType.Int);
        }

        /// <summary>
        /// Adds an output parameter to the parameter collection.
        /// </summary>
        /// <param name="parameters">The collection.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">The type of the Id, typically SqlDbType.Int.</param>
        /// <returns>The new SqlParameter object that was just added to the collection.</returns>
        public static SqlParameter AddOutputId
            (
            this SqlParameterCollection parameters,
            string parameterName,
            SqlDbType parameterType
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.Direction = ParameterDirection.Output;
            p.SqlDbType = parameterType;
            parameters.Add(p);
            return p;
        }
    }
}
