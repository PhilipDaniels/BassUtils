using System.Data;
using BassUtils.Oracle;

namespace BassUtils.OracleExamples.OracleUDTs;

class CallFunctionUsingObjects
{
    /// <summary>
    /// This calls a function which takes a parameter which is a SQL object - created using the
    /// "CREATE TYPE objPerson AS OBJECT" syntax - and also returns an object of the same type.
    /// </summary>
    public static void Execute(MenuItem menuItem)
    {
        UI.Heading(menuItem.Text);

        using var conn = Db.MakeConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "DemoUser.MyPackage.FunctionTakingObjects";
        UI.Calling(cmd);

        var returnParam = cmd.Parameters.AddReturnUdtObject("DemoUser.objPerson");
        var leela = People.MakeLeela();
        cmd.Parameters.AddUdtObject("pPerson", "DemoUser.objPerson", leela);


        UI.SubHeading("The person passed to the function");
        UI.Data(leela.ToString());

        cmd.ExecuteNonQuery();
        var result = (PersonRecord)returnParam.Value;
        UI.SubHeading("The person returned by the function");
        UI.Data(result.ToString());

        UI.PressAnyKey();
    }
}
