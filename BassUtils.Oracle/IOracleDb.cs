using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle;

/// <summary>
/// Simple helper base interface for Oracle connections.
/// <seealso cref="OracleDb"/>
/// </summary>
public interface IOracleDb
{
    /// <summary>
    /// Gets the connection string that is being used.
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Creates and opens a new connection.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>New Oracle connection.</returns>
    Task<OracleConnection> MakeConnectionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new connection, then starts a wrapped transaction on that connection.
    /// The wrapped transaction takes ownership of the connection, it will dispose the connection
    /// when it itself is dispoed.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Wrapped transaction.</returns>
    Task<WrappedTransaction> BeginWrappedTransactionAsync(CancellationToken cancellationToken = default);
}
