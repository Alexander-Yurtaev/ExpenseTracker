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

        return command switch
        {
            "add" => await AddCommand(parameters),
            "delete" => await DeleteCommand(parameters),
            "list" => await ListCommand(parameters),
            "summary" => await SummaryCommand(parameters),
            _ => new ResultMessage(false, $"Unknown command - *{command}")
        };
    }

    /// <summary>
    /// $ expense-tracker add --description "Lunch" --amount 20
    /// </summary>
    /// <param name="commandParameters"></param>
    private async Task<ResultMessage> AddCommand(Dictionary<string, string> commandParameters)
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
        await repository.CreateIfNotExists();
        var expenses = await repository.LoadAsync();

        var id = (expenses.Count > 0 ? expenses.Max(e => e.Id) : 0) + 1;
        var expense = new Expense(id, description, amount)
        {
            Date = DateTime.Now
        };
        expenses.Add(expense);
        await repository.SaveAsync(expenses);

        return new ResultMessage(true, expense.Id.ToString());
    }

    private async Task<ResultMessage> ListCommand(Dictionary<string, string> commandParameters)
    {
        var repository = new ExpenseRepository();
        await repository.CreateIfNotExists();
        var list = await repository.GetAllExpensesAsync();

        PrintExpensesTable(list);

        return await Task.FromResult(new ResultMessage(true, "List"));
    }

    private async Task<ResultMessage> DeleteCommand(Dictionary<string, string> commandParameters)
    {
        return await Task.FromResult(new ResultMessage(true, "Delete"));
    }

    private async Task<ResultMessage> SummaryCommand(Dictionary<string, string> commandParameters)
    {
        return await Task.FromResult(new ResultMessage(true, "Summary"));
    }

    public bool GetCommandAndParameters(string[] args, out string command, out Dictionary<string, string> commandParameters)
    {
        commandParameters = new Dictionary<string, string>();

        if (args.Length == 0 || (args.Length != 1 && int.IsOddInteger(args.Length - 1)))
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

    private void PrintCommands()
    {
        Console.WriteLine("expense-tracker add --description <description> --amount <amount>");
        Console.WriteLine("expense-tracker list");
        Console.WriteLine("expense-tracker summary");
        Console.WriteLine("expense-tracker delete --id <id>");
        Console.WriteLine("expense-tracker summary --month <month>");
    }

    private void PrintExpensesTable(List<Expense> list)
    {
        Console.WriteLine($"| {"ID", 2}  | {"Date", 10} | {"Description", 50} | {"Amount", 5} |");

        foreach (Expense expense in list)
        {
            Console.WriteLine($"| {expense.Id, 2}  | {expense.Date, 10:yyyy-mm-dd} | {expense.Description, 50} | {expense.Amount, 6} |");
        }
    }
}
