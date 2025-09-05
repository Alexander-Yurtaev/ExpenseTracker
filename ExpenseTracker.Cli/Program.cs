namespace ExpenseTracker.Cli;

public static class Program
{
    public static void Main(string[] args)
    {
        var manager = new ExpenseManager();
        manager.Execute(args);
    }
}