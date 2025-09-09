namespace ExpenseTracker.Cli;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var manager = new ExpenseManager();
        await manager.Execute(args);
    }
}