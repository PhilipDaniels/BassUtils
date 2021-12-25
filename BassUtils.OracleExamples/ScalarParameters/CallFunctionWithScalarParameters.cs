using System.Data;
using BassUtils.Oracle;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Spectre.Console;

namespace BassUtils.OracleExamples.ScalarParameters;

class CallFunctionWithScalarParameters
{
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.FunctionWithScalarParameters";
        UI.Calling(cmd);

        var returnParam = cmd.Parameters.AddReturn(OracleDbType.Decimal);

        var now = DateTime.UtcNow;
        var nowOffset = DateTimeOffset.UtcNow;

        // We can let type inference work for us.
        cmd.Parameters.Add("pInteger",10);
        cmd.Parameters.Add("pNumber", 12.2m);
        var pDate = cmd.Parameters.Add("pDate", now);
        var pTimestamp = cmd.Parameters.Add("pTimestamp", nowOffset);
        cmd.Parameters.Add("pChar", 'A');
        cmd.Parameters.Add("pNChar", '⌘');
        cmd.Parameters.Add("pAsciiString", "ascii");
        cmd.Parameters.Add("pUnicodeString", "Are we ✅ yet?");
        cmd.Parameters.Add("pFloat", 100.3);
        cmd.Parameters.Add("pDouble", 2222.33);
        var pOutputString = cmd.Parameters.Add("pOutputString", OracleDbType.NVarchar2, ParameterDirection.Output);
        pOutputString.Size = 255;

        // NUMBER maps to OracleDecimal, which has a larger precision than .Net decimal
        // so you can't cast directly to decimal. We get at the return value this way
        // rather than using ExecuteScalar().
        cmd.ExecuteNonQuery();
        var result = (decimal)(OracleDecimal)returnParam.Value;

        UI.SubHeading("pOutputString returned as an OUT parameter (Unicode may not show in console)");
        UI.Data($"{pOutputString.Value}");

        UI.SubHeading("DATE and TIMESTAMP precision");
        
        AnsiConsole.WriteLine("Oracle DATE type does not have milliseconds, so they will be stripped off.");
        AnsiConsole.WriteLine("Oracle TIMESTAMP type does have milliseconds.");
        UI.Data($"now       = {now:yyyy-MM-dd HH:mm:ss.fff}, pDate      = {(DateTime)pDate.Value:yyyy-MM-dd HH:mm:ss.fff}");
        UI.Data($"nowOffset = {nowOffset:yyyy-MM-dd HH:mm:ss.fff}, pTimestamp = {(DateTimeOffset)pTimestamp.Value:yyyy-MM-dd HH:mm:ss.fff}");

        UI.PressAnyKey();
    }
}
