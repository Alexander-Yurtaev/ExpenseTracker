using ExpenseTracker.Cli;
using ExpenseTracker.Cli.Models;
using FluentAssertions;
using System.Text.Json;

namespace ExpenseTracker.Tests
{
    public class ExpenseManagerTests
    {
        [Theory]
        [InlineData(2, "add", "--description", "Lunch", "--amount", "20")]
        [InlineData(2, "add", "--description", "Lunch", "--amount", "10")]
        public async Task GetCommandAndParametersTest(int paramsCount, params string[] args)
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();

            // Act
            manager.GetCommandsAndParameters(args, out string command, out var parameters);

            // Assert
            command.Should().BeEquivalentTo(args[0]);
            parameters.Count.Should().Be(paramsCount);

            parameters.ContainsKey(args[1]).Should().BeTrue();
            parameters[args[1]].Should().BeEquivalentTo(args[2]);

            parameters.ContainsKey(args[3]).Should().BeTrue();
            parameters[args[3]].Should().BeEquivalentTo(args[4]);
        }

        [Fact]
        public async Task AddCommandTest()
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();
            var args = new string[] { "add", "--description", "Milk", "--amount", "28" };

            // Act
            ResultMessage result = await manager.Execute(args);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue(result.Message);

            var expenses = await GetAllExpenses();

            expenses.Should().NotBeNullOrEmpty();
            expenses.Count.Should().Be(1);
            expenses.First().Id.Should().Be(1);
        }

        [Fact]
        public async Task AddMultipleCommandTest()
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();
            var argsList = GetMultipleArgs();

            // Act
            foreach (string[] args in argsList)
            {
                await manager.Execute(args);
            }

            // Assert
            var expenses = await GetAllExpenses();

            expenses.Should().NotBeNullOrEmpty();
            expenses.Count.Should().Be(argsList.Count);
            expenses.Select(e => e.Id).Distinct().Count().Should().Be(argsList.Count);
        }

        [Fact]
        public async Task SummaryCommandTest()
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();
            var argsList = GetMultipleArgs();

            foreach (string[] args in argsList)
            {
                await manager.Execute(args);
            }

            // Act
            var repository = new ExpenseRepository();
            var summary = await repository.GetSummaryAsync();

            // Assert
            summary.Should().Be(138);
        }

        [Fact]
        public async Task DeleteCommandTest()
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();
            var argsList = GetMultipleArgs();

            foreach (string[] args in argsList)
            {
                await manager.Execute(args);
            }

            // Act
            await manager.Execute(["delete", "--id", "2"]);

            // Assert
            var expenses = await GetAllExpenses();

            expenses.Should().NotBeNullOrEmpty();
            expenses.Count.Should().Be(2);
            expenses[0].Id.Should().Be(1);
            expenses[1].Id.Should().Be(3);
        }

        #region Private Methods

        private async Task RecreateFileDb()
        {
            var emptyList = new List<Expense>();
            var expensesJson = JsonSerializer.Serialize(emptyList);
            await File.WriteAllTextAsync(ExpenseRepository.FilePath, expensesJson);
        }

        private async Task<List<Expense>?> GetAllExpenses()
        {
            var expensesJson = await File.ReadAllTextAsync(ExpenseRepository.FilePath);
            var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson);
            return expenses;
        }

        private List<string[]> GetMultipleArgs()
        {
            var result = new List<string[]>();
            result.Add(["add", "--description", "Milk", "--amount", "28"]);
            result.Add(["add", "--description", "Sugar", "--amount", "30"]);
            result.Add(["add", "--description", "Water", "--amount", "80"]);

            return result;
        }

        #endregion
    }
}
