namespace ExpenseTracker.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var manager = new ExpenseManager();
        var result = await manager.Execute(args);

        Console.WriteLine(result.Message);
    }
}