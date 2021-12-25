using Oracle.ManagedDataAccess.Client;
using Spectre.Console;

namespace BassUtils.OracleExamples;

internal static class UI
{
    public static void Heading(string heading)
    {
        heading = (" " + heading + " ".Repeat(Console.WindowWidth)).Left(Console.WindowWidth);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[bold white on green]{heading}[/]");
        AnsiConsole.WriteLine();
    }

    public static void SubHeading(string subHeading)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[bold green]{subHeading}[/]");
    }

    internal static void Calling(OracleCommand cmd)
    {
        AnsiConsole.MarkupLine($"Calling [yellow]{cmd.CommandText}[/]");
    }

    public static void Data(string data)
    {
        AnsiConsole.WriteLine($"  {data}");
    }

    public static void MenuItem(string number, string text)
    {
        number = ("   " + number + ".").Right(4);
        AnsiConsole.MarkupLine($"[bold white] {number}[/] {text}");
    }

    public static void MenuItem(int number, string text)
    {
        MenuItem(number.ToString(), text);
    }

    public static void MenuItem(MenuItem menuItem)
    {
        MenuItem(menuItem.Number.ToString(), menuItem.Text);
    }

    public static void PressAnyKey()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
