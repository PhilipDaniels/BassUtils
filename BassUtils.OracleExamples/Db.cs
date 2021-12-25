using Oracle.ManagedDataAccess.Client;

namespace BassUtils.OracleExamples;

public static class Db
{
    public static OracleConnection MakeConnection()
    {
        return MakeConnectionAsync().GetAwaiter().GetResult();
    }

    public async static Task<OracleConnection> MakeConnectionAsync()
    {
        // You can discover the serviceName by doing "select * from global_name" in SQLPlus.
        var connStr = GetConnStr("10.0.2.2", 1521, "ORCLCDB.localdomain", "demouser", "demouser");
        var conn = new OracleConnection(connStr);

        await conn.OpenAsync().ConfigureAwait(false);
        return conn;
    }

    /// <summary>
    /// Returns a connection string that should work without having TNS setup.
    /// </summary>
    private static string GetConnStr(string hostNameOrIp, int port, string serviceName, string userId, string password)
    {
        return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={hostNameOrIp})(PORT={port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={serviceName}))); User Id={userId}; Password={password};";
    }
}
