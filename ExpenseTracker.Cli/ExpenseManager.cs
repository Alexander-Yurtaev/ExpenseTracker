namespace ExpenseTracker.Cli;

public class ExpenseManager
{
    public void Execute(string[] args)
    {
        if (!GetCommandAndParameters(args, out var command, out var parameters))
        {
            Console.WriteLine("Wrong value(s) in the args.");
            return;
        }

        switch (command)
        {
            case "add":
                AddCommand(parameters);
                break;
            case "delete":
                DeleteCommand(parameters);
                break;
            case "list":
                ListCommand(parameters);
                break;
            case "summary":
                SummaryCommand(parameters);
                break;
            default:
                Console.WriteLine($"Unknown command - *{command}");
                break;
        }
    }

    private static void AddCommand(Dictionary<string, string> commandParameters)
    {
        Console.WriteLine("Add");
    }

    private static void DeleteCommand(Dictionary<string, string> commandParameters)
    {
        Console.WriteLine("Delete");
    }

    private static void ListCommand(Dictionary<string, string> commandParameters)
    {
        Console.WriteLine("List");
    }
    private static void SummaryCommand(Dictionary<string, string> commandParameters)
    {
        Console.WriteLine("Summary");
    }

    private static bool GetCommandAndParameters(string[] args, out string command, out Dictionary<string, string> commandParameters)
    {
        commandParameters = new Dictionary<string, string>();

        if (args.Length <= 1 || int.IsOddInteger(args.Length - 1))
        {
            Console.WriteLine("You need to enter a command and, if it is necessary, parameter(s): its name and value.");
            Console.WriteLine("");
            PrintCommands();

            command = "";

            return false;
        }

        command = args[0];
        commandParameters = new Dictionary<string, string>();

        for (int i = 0; i < args.Length / 2; i++)
        {
            commandParameters.Add(args[2 * i + 1], args[2 * i + 2]);
        }

        return true;
    }

    private static void PrintCommands()
    {
        Console.WriteLine("expense-tracker add --description <description> --amount <amount>");
        Console.WriteLine("expense-tracker list");
        Console.WriteLine("expense-tracker summary");
        Console.WriteLine("expense-tracker delete --id <id>");
        Console.WriteLine("expense-tracker summary --month <month>");
    }
}
