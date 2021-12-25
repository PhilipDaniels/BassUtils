using BassUtils.OracleExamples;
using Spectre.Console;

while (true)
{
    AnsiConsole.Clear();
    var action = DisplayMenu();

    try
    {
        AnsiConsole.Clear();
        action();
    }
    catch (Exception ex)
    {
        AnsiConsole.WriteException(ex);
    }
}

Action DisplayMenu()
{
    UI.Heading("Oracle ODP.Net Examples");
    
    foreach (var mi in MenuItem.MenuItems)
    {
        UI.MenuItem(mi);
    }
    AnsiConsole.WriteLine();

    var choice = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter choice: ")
                    .ValidationErrorMessage("Not a valid choice, try again")
                    .Validate(choice =>
                    {
                        if (MenuItem.MenuItems.Any(mi => mi.Number == choice))
                            return ValidationResult.Success();
                        else
                            return ValidationResult.Error();
                    }));

    return MenuItem.MenuItems.Single(mi => mi.Number.ToString() == choice).MenuAction;
}
