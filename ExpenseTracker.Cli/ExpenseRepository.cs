using System.Text.Json;
using ExpenseTracker.Cli.Models;

namespace ExpenseTracker.Cli;

public class ExpenseRepository
{
    private const string FilePath = "ExpenseDb.json";

    public async Task CreateIfNotExists()
    {
        if (!File.Exists(FilePath))
        {
            // Create empty collection
            var emptyList = new List<Expense>();

            await SaveAsync(emptyList);
        }
    }

    public async Task<List<Expense>> LoadAsync()
    {
        await CreateIfNotExists();

        var expensesJson = await File.ReadAllTextAsync(FilePath);
        var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson) ?? [];
        return expenses;
    }

    public async Task<int> GetNextIdAsync()
    {
        await CreateIfNotExists();

        var expenses = await LoadAsync();
        if (expenses.Count == 0)
        {
            return 1;
        }

        return expenses.Max(e => e.Id) + 1; 
    }

    public async Task SaveAsync(List<Expense> expenses)
    {
        var expensesJson = JsonSerializer.Serialize(expenses);
        await File.WriteAllTextAsync(FilePath, expensesJson);
    }
}