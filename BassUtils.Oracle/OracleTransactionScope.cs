/*
using Dawn;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle
{
    /// <summary>
    /// A class that groups an <c>OracleConnection</c> and an <c>OracleTransaction</c>
    /// together, so that they can both be kept alive for the same time and disposed
    /// at the same time.
    /// </summary>
    public sealed class OracleTransactionScope : IDisposable
    {
        bool disposed = false;

        /// <summary>
        /// Gets the nested connection.
        /// </summary>
        public OracleConnection Connection { get; private set; }

        /// <summary>
        /// Gets the nested transaction.
        /// </summary>
        public OracleTransaction Transaction { get; private set; }

        /// <summary>
        /// Construct a new <c>OracleTransactionScope</c>. It is expected that the <paramref name="oracleTransaction"/>
        /// was created from the <paramref name="oracleConnection"/>.
        /// </summary>
        /// <param name="oracleConnection">Oracle connection. Must not be null.</param>
        /// <param name="oracleTransaction">Oracle transaction. Must not be null.</param>
        public OracleTransactionScope(OracleConnection oracleConnection, OracleTransaction oracleTransaction)
        {
            Connection = Guard.Argument(oracleConnection, nameof(oracleConnection)).NotNull();
            Transaction = Guard.Argument(oracleTransaction, nameof(oracleTransaction)).NotNull();
        }

        /// <summary>
        /// Creates a new command tied to the scope. The command is created from the connection
        /// and has its transaction set to the scope's transaction.
        /// </summary>
        /// <returns>New Oracle command object.</returns>
        public OracleCommand CreateCommand()
        {
            var cmd = Connection.CreateCommand();
            //cmd.Transaction = Transaction;
            return cmd;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
        }

        /// <summary>
        /// Commits the transaction, logging a success or failure message.
        /// </summary>
        /// <remarks>
        /// If the commits throws an exception a message is logged and then the exception is rethrown.
        /// </remarks>
        public void Commit<T>(ILogger<T> logger, string msg = "")
        {
            try
            {
                Transaction.Commit();
                logger.LogInformation("Successfully committed {}", msg);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Exception while committing {}", msg);
                throw;
            }
        }

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Transaction.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <remarks>
        /// If the commit throws an exception a message is logged and then the exception is rethrown.
        /// </remarks>
        /// <typeparam name="T">Type for the logger.</typeparam>
        /// <param name="logger">A logger.</param>
        /// <param name="msg">Extra string to include in the log messages.</param>
        /// <param name="cancellationToken">Cancellation</param>
        public async Task CommitAsync<T>(ILogger<T> logger, string msg = "", CancellationToken cancellationToken = default)
        {
            try
            {
                await Transaction.CommitAsync(cancellationToken);
                logger.LogInformation("Successfully committed {}", msg);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Exception while committing {}", msg);
                throw;
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
        }

        /// <summary>
        /// Rolls back the transaction, logging a success or failure message.
        /// </summary>
        /// <remarks>
        /// If the rollback throws an exception a message is logged and then the exception is rethrown.
        /// </remarks>
        public void Rollback<T>(ILogger<T> logger, string msg = "")
        {
            try
            {
                Transaction.Rollback();
                logger.LogInformation("Successfully rolled back {}", msg);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Exception while rolling back {}", msg);
                throw;
            }
        }

        /// <summary>
        /// Rolls back the transaction to a savepoint name.
        /// </summary>
        /// <param name="savepointName">Savepoint name.</param>
        public void Rollback(string savepointName)
        {
            Transaction.Rollback(savepointName);
        }

        /// <summary>
        /// Rolls back the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return Transaction.RollbackAsync(cancellationToken);
        }

        /// <summary>
        /// Rolls back the transaction asynchronously.
        /// </summary>
        /// <remarks>
        /// If the rollback throws an exception a message is logged and then the exception is rethrown.
        /// </remarks>
        /// <typeparam name="T">Type for the logger.</typeparam>
        /// <param name="logger">A logger.</param>
        /// <param name="msg">Extra string to include in the log messages.</param>
        /// <param name="cancellationToken">Cancellation</param>
        public async Task RollbackAsync<T>(ILogger<T> logger, string msg = "", CancellationToken cancellationToken = default)
        {
            try
            {
                await Transaction.RollbackAsync(cancellationToken);
                logger.LogInformation("Successfully rolled back {}", msg);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Exception while rolling back {}", msg);
                throw;
            }
        }

        /// <summary>
        /// Creates a savepoint.
        /// </summary>
        /// <param name="savepointName">Savepoint name.</param>
        public void Save(string savepointName)
        {
            Transaction.Save(savepointName);
        }

        /// <summary>
        /// Disposes the transaction and the connection.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;

                if (Transaction != null)
                {
                    Transaction.Dispose();
                }

                if (Connection != null)
                {
                    Connection.Dispose();
                }

                GC.SuppressFinalize(this);
            }
        }
    }
}
*/