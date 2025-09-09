namespace ExpenseTracker.Cli.Models;

public record Expense
{
    public Expense(int id, string description, int amount)
    {
        if (id <= 0) throw new ArgumentException("ID must be positive.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.");
        if (amount < 0) throw new ArgumentException("Summary cannot be negative.");

        Id = id;
        Date = DateTime.Now;
        Description = description;
        Amount = amount;
    }

    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Description { get; init; }
    public int Amount { get; init; }
}