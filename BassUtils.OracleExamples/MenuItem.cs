using BassUtils.OracleExamples.ClientSideArrayBinding;
using BassUtils.OracleExamples.OracleUDTs;
using BassUtils.OracleExamples.RetrievingRecordsets;
using BassUtils.OracleExamples.ScalarParameters;

namespace BassUtils.OracleExamples;

class MenuItem
{
    public string Number { get; private set; }
    public string Text { get; private set; }
    public Action MenuAction { get; private set; }

    public static MenuItem[] MenuItems = new MenuItem[] {
        new()
        {
            Number = "1",
            Text = "Call a function using scalar parameters",
            MenuAction = () => CallFunctionWithScalarParameters.Execute(MenuItems[0])
        },
        new()
        {
            Number = "2",
            Text = "Call a function using arrays of scalars (e.g. to pass a list of ids)",
            MenuAction = () => CallFunctionWithArraysOfScalarParameters.Execute(MenuItems[1])
        },
        new()
        {
            Number = "3",
            Text = "Call a procedure using client-side array binding (to reduce network calls)",
            MenuAction = () => CallProcedureUsingArrayBinding.Execute(MenuItems[2])
        },
        new()
        {
            Number = "4",
            Text = "Call a function that returns a SYS_REFCURSOR (recordset)",
            MenuAction = () => CallFunctionReturningRefCursor.Execute(MenuItems[3])
        },
        new()
        {
            Number = "5",
            Text = "Call a procedure that returns 2 SYS_REFCURSORs (recordsets)",
            MenuAction = () => CallProcedureReturningTwoRefCursors.Execute(MenuItems[4])
        },
        new()
        {
            Number = "6",
            Text = "Call a function using OBJECTs (UDTs)",
            MenuAction = () => CallFunctionUsingObjects.Execute(MenuItems[5])
        },
        new()
        {
            Number = "7",
            Text = "Call a function using TABLES OF OBJECTS (UDTs)",
            MenuAction = () => CallFunctionUsingTablesOfObjects.Execute(MenuItems[6])
        },
        new()
        {
            Number = "8",
            Text = "Call a procedure using null and non-null UDTs",
            MenuAction = () => CallProcedureUsingNullableObjects.Execute(MenuItems[7])
        },
        new()
        {
            Number = "x",
            Text = "Exit",
            MenuAction = () => Environment.Exit(0)
        }
    };
}
