using System.Text.Json;
using ExpenseTracker.Cli.Models;

namespace ExpenseTracker.Cli;

public class ExpenseRepository
{
    public const string FilePath = "ExpenseDb.json";

    public async Task<List<Expense>> GetAllExpensesAsync()
    {
        var expenses = await LoadAsync();
        return expenses;
    }

    public async Task SaveAsync(List<Expense> expenses)
    {
        var expensesJson = JsonSerializer.Serialize(expenses);
        await File.WriteAllTextAsync(FilePath, expensesJson);
    }

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
        try
        {
            var expensesJson = await File.ReadAllTextAsync(FilePath);
            var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson) ?? [];
            return expenses;
        }
        catch (JsonException e)
        {
            throw new Exception("Error! Loading data", e);
        }
    }

    public async Task<int> GetSummaryAsync(int? month=null)
    {
        var list = await LoadAsync();

        if (month.HasValue)
        {
            var year = DateTime.Now.Year;
            list = list.Where(l => l.Date.Month == month.Value && l.Date.Year == year).ToList();
        }

        return list.Sum(l => l.Amount);
    }
}