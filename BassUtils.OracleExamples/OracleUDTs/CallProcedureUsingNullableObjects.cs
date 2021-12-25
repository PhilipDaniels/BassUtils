using System.Data;
using BassUtils.Oracle;

namespace BassUtils.OracleExamples.OracleUDTs;

class CallProcedureUsingNullableObjects
{
    /// <summary>
    /// This demonstrates how to pass and return nullable OBJECTs and deal with
    /// TABLES that have null objects within them.
    /// </summary>
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.ProcWithNullableObjects";
        UI.Calling(cmd);

        // We use the special Null factory method here, or we can cast to a null of the correct type. Both work.
        // (The cast is required because of the generic constraint on AddUdtObject).
        var pPerson1 = cmd.Parameters.AddUdtObject("pPerson1", "DemoUser.objPerson", PersonRecord.Null, ParameterDirection.InputOutput);
        var pPerson2 = cmd.Parameters.AddUdtObject("pPerson2", "DemoUser.objPerson", (PersonRecord)null, ParameterDirection.InputOutput);

        // The third parameter is a table of PersonRecord. This is expressed in the special
        // C# wrapper class that we had to write to tell Oracle how to serialize these things.
        // We insert a couple of null objects to demonstrate how to handle them.
        var people = People.MakeFuturama().ToList();
        people.Add(null);
        people.Add(null);
        var peopleRecordArray = new PersonRecordArray() { Rows = people.ToArray() };
        var pPeople = cmd.Parameters.AddUdtArray("pPeople", "DemoUser.tblPerson", peopleRecordArray);


        UI.SubHeading("The people passed to the procedure");
        DumpPeople(null, null, peopleRecordArray.Rows);

        cmd.ExecuteNonQuery();

        var returnedPeople = (PersonRecordArray)pPeople.Value;
        UI.SubHeading("The people returned by the procedure");
        var p1 = (PersonRecord)pPerson1.Value;
        var p2 = (PersonRecord)pPerson2.Value;
        DumpPeople(p1, p2, returnedPeople.Rows);

        UI.PressAnyKey();
    }

    private static void DumpPeople(PersonRecord p1, PersonRecord p2, IEnumerable<PersonRecord> people)
{
        UI.Data($"P1 = {PersonString(p1)}");
        UI.Data($"P2 = {PersonString(p2)}");

        int i = 0;
        foreach (var person in people)
        {
            UI.Data($"People[{i}] = {PersonString(person)}");
            i++;
        }
    }

    private static string PersonString(PersonRecord p)
    {
        // Note the special IsNull property you have to check on the way out.
        // It is an error to compare with " == null" (though that works
        // when this method is called with parameters before they are passed
        // to the proc).
        if (p == null || p.IsNull)
            return "";
        else
            return p.ToString();
    }
}
