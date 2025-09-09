namespace ExpenseTracker.Cli.Models;

public class Expense
{
    public Expense(int id, string description, int amount)
    {
        Id = id;
        Description = description;
        Amount = amount;
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}