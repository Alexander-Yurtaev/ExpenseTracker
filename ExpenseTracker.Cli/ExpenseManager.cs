using ExpenseTracker.Cli.Models;

namespace ExpenseTracker.Cli;

public class ExpenseManager
{
    public async Task<ResultMessage> Execute(string[] args)
    {
        if (!GetCommandAndParameters(args, out var command, out var parameters))
        {
            return new ResultMessage(false, "Wrong value(s) in the args.");
        }

        switch (command)
        {
            case "add":
                return await AddCommand(parameters);
            case "delete":
                return await DeleteCommand(parameters);
            case "list":
                return await ListCommand(parameters);
            case "summary":
                return await SummaryCommand(parameters);
            default:
                return new ResultMessage(false, $"Unknown command - *{command}");
        }
    }

    /// <summary>
    /// $ expense-tracker add --description "Lunch" --amount 20
    /// </summary>
    /// <param name="commandParameters"></param>
    private static async Task<ResultMessage> AddCommand(Dictionary<string, string> commandParameters)
    {
        if (!commandParameters.TryGetValue("--description", out var description))
        {
            return new ResultMessage(false, $"Parameter '--description' was not found.");
        }

        if (!commandParameters.TryGetValue("--amount", out var amountString))
        {
            return new ResultMessage(false, $"Parameter '--amount' was not found.");
        }

        if (!Int32.TryParse(amountString, out var amount))
        {
            return new ResultMessage(false, $"'--amount' parameter must be int.");
        }

        var repository = new ExpenseRepository();
        var id = await repository.GetNextIdAsync();
        var expense = new Expense(id, description, amount)
        {
            Date = DateTime.Now
        };
        await repository.SaveAsync([expense]);

        return new ResultMessage(true, expense.Id.ToString());
    }

    private static async Task<ResultMessage> DeleteCommand(Dictionary<string, string> commandParameters)
    {
        return new ResultMessage(true, "Delete");
    }

    private static async Task<ResultMessage> ListCommand(Dictionary<string, string> commandParameters)
    {
        return new ResultMessage(true, "List");
    }
    private static async Task<ResultMessage> SummaryCommand(Dictionary<string, string> commandParameters)
    {
        return new ResultMessage(true, "Summary");
    }

    public static bool GetCommandAndParameters(string[] args, out string command, out Dictionary<string, string> commandParameters)
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
