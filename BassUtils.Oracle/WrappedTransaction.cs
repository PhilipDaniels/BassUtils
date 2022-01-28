using Dawn;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle;

/// <summary>
/// Wraps a database connection and transaction up together
/// and properly implements <c>IDisposable</c> on the pair.
/// The Oracle data provider seems to have trouble managing lifetimes
/// correctly, using this class avoids problems with closing and
/// disposing of connections and transactions.
/// </summary>
public sealed class WrappedTransaction : IDisposable, IAsyncDisposable
{
    bool disposed;
    public OracleConnection Connection { get; init; }
    public OracleTransaction Transaction { get; init; }

    /// <summary>
    /// Construct a new <seealso cref="WrappedTransaction"/>.
    /// </summary>
    /// <param name="connection">Connection that originated the transaction. Must not be null.</param>
    /// <param name="transaction">Transaction. Must not be null.</param>
    public WrappedTransaction(OracleConnection connection, OracleTransaction transaction)
    {
        Connection = Guard.Argument(connection, nameof(connection)).NotNull(); 
        Transaction = Guard.Argument(transaction, nameof(transaction)).NotNull();
    }

    /// <summary>
    /// Disposes of both the transaction and the connection.
    /// </summary>
    public void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            if (Transaction != null)
                Transaction.Dispose();
            if (Connection != null)
                Connection.Dispose();
        }
    }

    /// <summary>
    /// Disposes of both the transaction and the connection asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!disposed)
        {
            disposed = true;
            if (Transaction != null)
                await Transaction.DisposeAsync();
            if (Connection != null)
                await Connection.DisposeAsync();
        }
    }

    /// <summary>
    /// Commits the transaction.
    /// </summary>
    public void Commit()
    {
        Transaction.Commit();
    }

    /// <summary>
    /// Commits the transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Task.</returns>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Transaction.CommitAsync(cancellationToken).ConfigureAwait(false); ;
    }

    /// <summary>
    /// Commits the transaction asynchronously and logs a message that it has done so, or
    /// logs the exception if the commit fails.
    /// </summary>
    /// <typeparam name="T">Generic type for the logger.</typeparam>
    /// <param name="logger">Logger instance.</param>
    /// <param name="messageText">Optional extra text to include in the log message.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Task.</returns>
    public async Task CommitAsync<T>(ILogger<T> logger, string messageText = "", CancellationToken cancellationToken = default)
    {
        Guard.Argument(logger, nameof(logger)).NotNull();

        await Transaction.CommitAsync(logger, messageText, cancellationToken).ConfigureAwait(false); ;
    }

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    public void Rollback()
    {
        Transaction.Rollback();
    }

    /// <summary>
    /// Rollsback the transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Task.</returns>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await Transaction.RollbackAsync(cancellationToken).ConfigureAwait(false); ;
    }

    /// <summary>
    /// Rolls back the transaction asynchronously and logs a message that it has done so, or
    /// logs the exception if the rollback fails.
    /// </summary>
    /// <typeparam name="T">Generic type for the logger.</typeparam>
    /// <param name="logger">Logger instance.</param>
    /// <param name="messageText">Optional extra text to include in the log message.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Task.</returns>
    public async Task RollbackAsync<T>(ILogger<T> logger, string messageText = "", CancellationToken cancellationToken = default)
    {
        Guard.Argument(logger, nameof(logger)).NotNull();

        await Transaction.RollbackAsync(logger, messageText, cancellationToken).ConfigureAwait(false); ;
    }
}
