using System.Data;
using BassUtils.Oracle;

namespace BassUtils.OracleExamples.OracleUDTs;

class CallFunctionUsingTablesOfObjects
{
    /// <summary>
    /// This calls a function which takes a parameter which is a SQL table - created using the
    /// "CREATE TYPE tblPerson AS TABLE OF objPerson" syntax - and also returns an object of the same type.
    /// </summary>
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.FunctionTakingTablesOfObjects";
        UI.Calling(cmd);

        // The return parameter is a table of UDTs.
        var returnParam = cmd.Parameters.AddReturnUdtArray("DemoUser.tblPerson");

        // The first parameter is a table of PersonRecord. This is expressed in the special
        // C# wrapper class that we had to write to tell Oracle how to serialize these things.
        var people = new PersonRecordArray() { Rows = People.MakeFuturama() };
        cmd.Parameters.AddUdtArray("pPeople", "DemoUser.tblPerson", people);

        // The second parameter is an associative array of decimals, the type being declared in a package.
        // This is the only way to pass an array of primitive scalars via ODP.Net.
        // Note that the function is still a schema-level function.
        cmd.Parameters.AddAssociativeArray("pAgeMultipliers", new[] { 1.0m, 1.1m, 1.2m, 4.4m });


        UI.SubHeading("The people passed to the function");
        foreach (var person in people.Rows)
        {
            UI.Data(person.ToString());
        }


        cmd.ExecuteNonQuery();


        var returnedPeople = (PersonRecordArray)returnParam.Value;
        UI.SubHeading("The people returned by the function");
        foreach (var person in returnedPeople.Rows)
        {
            UI.Data(person.ToString());
        }

        UI.PressAnyKey();
    }
}
