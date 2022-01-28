using System.Data.Common;
using Dawn;
using Microsoft.Extensions.Logging;

namespace BassUtils.Oracle;

/// <summary>
/// Extension methods for the <c>DbTransaction</c> class.
/// </summary>
/// <remarks>
/// This class  cannot be moved into BassUtils yet because netstandard2.0 version of DbTransaction
/// does not support the async methods.</remarks>
public static class DbTransactionExtensions
{
    /// <summary>
    /// Commits the transaction, logging a success or failure message.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="logger">A logger.</param>
    /// <param name="msg">Extra string to include in the log messages.</param>
    /// <remarks>
    /// If the commits throws an exception a message is logged and then the exception is rethrown.
    /// </remarks>
    public static void Commit<T>(this DbTransaction transaction, ILogger<T> logger, string msg = "")
    {
        Guard.Argument(transaction, nameof(transaction)).NotNull();
        Guard.Argument(logger, nameof(logger)).NotNull();

        try
        {
            transaction.Commit();
            logger.LogInformation("Successfully committed {0}", msg);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while committing {0}", msg);
            throw;
        }
    }

    /// <summary>
    /// Commits the transaction asynchronously, logging a success or failure message.
    /// </summary>
    /// <remarks>
    /// If the commit throws an exception a message is logged and then the exception is rethrown.
    /// </remarks>
    /// <typeparam name="T">Type for the logger.</typeparam>
    /// <param name="transaction">The transaction.</param>
    /// <param name="logger">A logger.</param>
    /// <param name="msg">Extra string to include in the log messages.</param>
    /// <param name="cancellationToken">Cancellation</param>
    public static async Task CommitAsync<T>(this DbTransaction transaction, ILogger<T> logger, string msg = "", CancellationToken cancellationToken = default)
    {
        Guard.Argument(transaction, nameof(transaction)).NotNull();
        Guard.Argument(logger, nameof(logger)).NotNull();

        try
        {
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Successfully committed {0}", msg);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while committing {0}", msg);
            throw;
        }
    }

    /// <summary>
    /// Rolls back the transaction, logging a success or failure message.
    /// </summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="logger">A logger.</param>
    /// <param name="msg">Extra string to include in the log messages.</param>
    /// <remarks>
    /// If the rollback throws an exception a message is logged and then the exception is rethrown.
    /// </remarks>
    public static void Rollback<T>(this DbTransaction transaction, ILogger<T> logger, string msg = "")
    {
        Guard.Argument(transaction, nameof(transaction)).NotNull();
        Guard.Argument(logger, nameof(logger)).NotNull();

        try
        {
            transaction.Rollback();
            logger.LogInformation("Successfully rolled back {0}", msg);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Exception while rolling back {0}", msg);
            throw;
        }
    }

    /// <summary>
    /// Rolls back the transaction asynchronously.
    /// </summary>
    /// <remarks>
    /// If the rollback throws an exception a message is logged and then the exception is rethrown.
    /// </remarks>
    /// <typeparam name="T">Type for the logger.</typeparam>
    /// <param name="transaction">The transaction.</param>
    /// <param name="logger">A logger.</param>
    /// <param name="msg">Extra string to include in the log messages.</param>
    /// <param name="cancellationToken">Cancellation</param>
    public static async Task RollbackAsync<T>(this DbTransaction transaction, ILogger<T> logger, string msg = "", CancellationToken cancellationToken = default)
    {
        Guard.Argument(transaction, nameof(transaction)).NotNull();
        Guard.Argument(logger, nameof(logger)).NotNull();

        try
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false); ;
            logger.LogInformation("Successfully rolled back {0}", msg);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Exception while rolling back {0}", msg);
            throw;
        }
    }
}
