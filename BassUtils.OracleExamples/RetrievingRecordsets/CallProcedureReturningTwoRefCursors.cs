using System.Data;
using System.Text;
using BassUtils.Oracle;

namespace BassUtils.OracleExamples.RetrievingRecordsets;

class CallProcedureReturningTwoRefCursors
{
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.GetPeopleNames";
        UI.Calling(cmd);

        var pFirstNames = cmd.Parameters.AddRefCursor("pFirstNames");
        var pLastNames = cmd.Parameters.AddRefCursor("pLastNames");

        // We can actually get at the resultsets in two ways.

        UI.SubHeading("Technique 1: Using a DataReader extracted from the parameter");
        cmd.ExecuteNonQuery();
        using var rdrFirstNames = pFirstNames.GetValueAsDataReader();
        while (rdrFirstNames.Read())
        {
            UI.Data("FIRST: " + rdrFirstNames.GetString("FirstName"));
        }
        using var rdrLastNames = pLastNames.GetValueAsDataReader();
        while (rdrLastNames.Read())
        {
            UI.Data("LAST : " + rdrLastNames.GetString("LastName"));
        }

        UI.SubHeading("Technique 2: Using a DataReader created by ExecuteReader() and NextResult()");
        using var rdr1 = cmd.ExecuteReader();
        while (rdr1.Read())
        {
            UI.Data("FIRST: " + rdr1.GetString("FirstName"));
        }
        rdr1.NextResult();
        while (rdr1.Read())
        {
            UI.Data("LAST : " + rdr1.GetString("LastName"));
        }

        UI.PressAnyKey();
    }
}
