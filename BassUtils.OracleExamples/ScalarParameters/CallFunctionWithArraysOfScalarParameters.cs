using System.Data;
using BassUtils.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.OracleExamples.ScalarParameters;

// Based on
// https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872

class CallFunctionWithArraysOfScalarParameters
{
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.FunctionTakingArraysOfScalars";
        UI.Calling(cmd);

        // Demonstrate the 'difficult' case, how to get back an array of strings.
        var pReturnParam = cmd.Parameters.AddReturnAssociativeStringArray(arraySize: 10, maxStringLength: 30);
        // This is how to get back an array of ints.
        // var returnParam = cmd.Parameters.AddReturnAssocArray(10, OracleDbType.Int32);

        cmd.Parameters.AddAssociativeArray("pFloats", new float[] { 1.1f, 2.2f });

        var dates = new DateTime[]
        {
            DateTime.Now,
            DateTime.Now.AddHours(5),
            DateTime.Now.AddHours(-125),
        };
        cmd.Parameters.AddAssociativeArray("pDates", dates);

        cmd.Parameters.AddAssociativeArray("pNumbers", new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });
        cmd.Parameters.AddAssociativeArray("pStrings", new string[] { "Hello", "From Oracle" });
        var pNumbersOut = cmd.Parameters.AddOutputAssociativeArray("pNumbersOut", 10, OracleDbType.Int32);
        var pStringsOut = cmd.Parameters.AddOutputAssociativeStringArray("pStringsOut", arraySize: 10, maxStringLength: 30);

        cmd.ExecuteNonQuery();

        UI.SubHeading("pNumbersOut returned as an OUT parameter");
        UI.Data(string.Join(",", pNumbersOut.GetValueAsDecimalList()));
        
        UI.SubHeading("pStringsOut returned as an OUT parameter");
        UI.Data(string.Join(",", pStringsOut.GetValueAsStringList()));

        UI.SubHeading("pReturnParam returned as the function RETURN value");
        UI.Data(string.Join(",", pReturnParam.GetValueAsStringList()));

        UI.PressAnyKey();
    }
}
