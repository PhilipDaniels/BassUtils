using System.Data;

namespace BassUtils
{
    public static class IDbConnectionExtensions
    {
        /// <summary>
        /// Gets the schema of a table (or view) using the <c>IDataReader.GetSchemaTable()</c>
        /// method. This is good for basic information, but will not be full-fidelity compared
        /// to what is available from SQL server.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>DataTable containing the schema for the SQL table.</returns>
        public static DataTable GetSchemaTable(this IDbConnection connection, string tableName)
        {
            connection.ThrowIfNull("connection");
            tableName.ThrowIfNullOrEmpty("tableName");

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from " + tableName + " where 1 == 0";

                using (var rdr = cmd.ExecuteReader())
                {
                    return rdr.GetSchemaTable();
                }
            }
        }
    }
}
