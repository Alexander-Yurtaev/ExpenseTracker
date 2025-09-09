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
        public void GetCommandAndParametersTest(int paramsCount, params string[] args)
        {
            // Arrange

            // Act
            ExpenseManager.GetCommandAndParameters(args, out string command, out var parameters);

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
            var manager = new ExpenseManager();
            var args = new string[] { "add", "--description", "Milk", "--amount", "28" };

            // Act
            ResultMessage result = await manager.Execute(args);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue(result.Message);
        }
    }
}
