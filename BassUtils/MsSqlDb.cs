using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BassUtils
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class MsSqlDb : IMsSqlDb
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Initialise a new instance of the <see cref="MsSqlDb"/>.
        /// </summary>
        /// <param name="connectionString">Connection string to use.</param>
        public MsSqlDb(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<SqlConnection> MakeConnectionAsync(CancellationToken cancellationToken = default)
        {
            var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            return conn;
        }
    }
}
