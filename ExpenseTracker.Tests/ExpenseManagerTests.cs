using System.Text.Json;
using ExpenseTracker.Cli;
using ExpenseTracker.Cli.Models;
using FluentAssertions;

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
            manager.GetCommandAndParameters(args, out string command, out var parameters);

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
            var idString = result.Message;
            if (!int.TryParse(idString, out var id))
            {
                Assert.Fail("Id must be int.");
            }

            id.Should().Be(1);
        }

        [Fact]
        public async Task AddMultipleCommandsTest()
        {
            // Arrange
            await RecreateFileDb();
            var manager = new ExpenseManager();
            var argsSet = new string[2][];

            argsSet[0] = ["add", "--description", "Milk", "--amount", "28"];
            argsSet[1] = ["add", "--description", "Sugar", "--amount", "30"];

            var resultSet = new ResultMessage[2];

            // Act
            var index = 0;
            foreach (string[] args in argsSet)
            {
                resultSet[index] = await manager.Execute(args);
                index++;
            }

            // Assert
            var expensesJson = await File.ReadAllTextAsync(ExpenseRepository.FilePath);
            var expenses = JsonSerializer.Deserialize<List<Expense>>(expensesJson);

            expenses.Should().NotBeNullOrEmpty();
            expenses.Count.Should().Be(2);
        }

        private async Task RecreateFileDb()
        {
            var emptyList = new List<Expense>();
            var expensesJson = JsonSerializer.Serialize(emptyList);
            await File.WriteAllTextAsync(ExpenseRepository.FilePath, expensesJson);
        }
    }
}
