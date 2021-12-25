using System.Data;
using BassUtils.Oracle;

namespace BassUtils.OracleExamples.RetrievingRecordsets;

class CallFunctionReturningRefCursor
{
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.GetPeople";
        UI.Calling(cmd);

        var prm = cmd.Parameters.AddReturnRefCursor();

        // We can actually get at the resultset in two ways.

        UI.SubHeading("Technique 1: Using a DataReader extracted from the RETURN parameter");
        cmd.ExecuteNonQuery();
        using var rdr = prm.GetValueAsDataReader();
        while (rdr.Read())
        {
            var first = rdr.GetString("FirstName");
            var last = rdr.GetString("LastName");
            var age = rdr.GetInt32("Age");
            UI.Data($"{first} {last} is {age} years old");
        }

        UI.SubHeading("Technique 2: Using a DataReader created by ExecuteReader()");
        using var rdr2 = cmd.ExecuteReader();
        while (rdr2.Read())
        {
            var first = rdr2.GetString("FirstName");
            var last = rdr2.GetString("LastName");
            var age = rdr2.GetInt32("Age");
            UI.Data($"{first} {last} is {age} years old");
        }

        UI.PressAnyKey();
    }
}
