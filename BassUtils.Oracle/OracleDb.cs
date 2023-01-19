using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle;

/// <summary>
/// Simple helper base class for Oracle connections.
/// </summary>
public abstract class OracleDb : IOracleDb
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string ConnectionString { get; init; }

    /// <summary>
    /// Initialise a new instance of <see cref="OracleDb"/>.
    /// </summary>
    /// <param name="connectionString">Connection string to use.</param>
    public OracleDb(string connectionString)
    {
        ConnectionString = connectionString;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual async Task<OracleConnection> MakeConnectionAsync(CancellationToken cancellationToken = default)
    {
        var conn = new OracleConnection(ConnectionString);
        await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
        return conn;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public virtual async Task<WrappedTransaction> BeginWrappedTransactionAsync(CancellationToken cancellationToken = default)
    {
        var conn = await MakeConnectionAsync(cancellationToken).ConfigureAwait(false);
        return await conn.BeginWrappedTransactionAsync(cancellationToken).ConfigureAwait(false);
    }
}
