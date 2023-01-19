using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BassUtils
{
    /// <summary>
    /// Simple base class for connections to MS SQL databases.
    /// </summary>
    public interface IMsSqlDb
    {
        /// <summary>
        /// The connection string that is in use.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Create and open a connection to the database.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>New, open SqlConnection</returns>
        Task<SqlConnection> MakeConnectionAsync(CancellationToken cancellationToken = default);
    }
}
