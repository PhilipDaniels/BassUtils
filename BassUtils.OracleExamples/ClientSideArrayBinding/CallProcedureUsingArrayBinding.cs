using System.Data;
using BassUtils.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.OracleExamples.ClientSideArrayBinding;

class CallProcedureUsingArrayBinding
{
    /// <summary>
    /// This calls a proc which takes 3 parameters, an int and 2 strings (so 3
    /// scalar parameters). Nevertheless, we create an OracleCommand which takes
    /// 3 arrays - importantly, all of which must be the same length!
    /// Executing the command sends the command text and the 3 arrays up to the server
    /// in one network call. The result should be 3 new rows in the Person table.
    /// 
    /// This works for procedures at schema-level or in packages.
    /// </summary>
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.InsertPerson";
        UI.Calling(cmd);

        var ages = new[] { 10, 20, 30 };
        cmd.Parameters.AddArray("pAge", OracleDbType.Int32, ParameterDirection.Input, ages);
        var firstNames = new[] { "Philip", "Hubert", "John" };
        cmd.Parameters.AddArray("pFirstName", ParameterDirection.Input, firstNames);
        var lastNames = new[] { "Fry", "Farnsworth", "Zoidberg" };
        cmd.Parameters.AddArray("pLastName", ParameterDirection.Input, lastNames);

        // 3 items in each array.
        cmd.ArrayBindCount = 3;

        cmd.ExecuteNonQuery();
        
        UI.Data("People inserted successfully");
        UI.PressAnyKey();
    }
}
