// filepath: c:\Users\kishi\Desktop\vscode\CSharp_ChatGPT\01_CSharp_ChatGPT\REPL\InteractiveShellTest.cs
using System;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace InteractiveShell.Tests
{
    public class CommandParserTests
    {
        [Fact]
        public void Parse_ValidInput_ReturnsCommandAndArgs()
        {
            var parser = new CommandParser();
            var (command, args) = parser.Parse("test arg1 arg2");

            Assert.Equal("test", command);
            Assert.Equal(new[] { "arg1", "arg2" }, args);
        }

        [Fact]
        public void Parse_EmptyInput_ReturnsEmptyCommandAndArgs()
        {
            var parser = new CommandParser();
            var (command, args) = parser.Parse("");

            Assert.Equal("", command);
            Assert.Empty(args);
        }

        [Fact]
        public void Parse_InputWithSpaces_ReturnsTrimmedCommandAndArgs()
        {
            var parser = new CommandParser();
            var (command, args) = parser.Parse("   test   arg1   arg2   ");

            Assert.Equal("test", command);
            Assert.Equal(new[] { "arg1", "arg2" }, args);
        }
    }

    public class CommandRegistryTests
    {
        [Fact]
        public void RegisterCommand_StoresCommand()
        {
            var registry = new CommandRegistry();
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Name).Returns("test");

            registry.RegisterCommand(mockCommand.Object);

            Assert.True(registry.HasCommand("test"));
            Assert.Equal(mockCommand.Object, registry.GetCommand("test"));
        }

        [Fact]
        public void GetCommand_UnregisteredCommand_ReturnsNull()
        {
            var registry = new CommandRegistry();

            Assert.Null(registry.GetCommand("unknown"));
        }
    }

    public class CommandExecutorTests
    {
        [Fact]
        public void Execute_ValidCommand_CallsExecuteOnCommand()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Name).Returns("test");

            var registry = new CommandRegistry();
            registry.RegisterCommand(mockCommand.Object);

            var executor = new CommandExecutor(registry);
            executor.Execute("test", new[] { "arg1", "arg2" });

            mockCommand.Verify(c => c.Execute(It.Is<string[]>(args => args.Length == 2 && args[0] == "arg1" && args[1] == "arg2")), Times.Once);
        }

        [Fact]
        public void Execute_InvalidCommand_PrintsErrorMessage()
        {
            var registry = new CommandRegistry();
            var executor = new CommandExecutor(registry);

            using var consoleOutput = new ConsoleOutput();
            executor.Execute("unknown", Array.Empty<string>());

            Assert.Contains("Unknown command. Type 'help' for a list of commands.", consoleOutput.GetOutput());
        }
    }

    public class HelpCommandTests
    {
        [Fact]
        public void Execute_PrintsAvailableCommands()
        {
            var registry = new CommandRegistry();
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Name).Returns("test");
            registry.RegisterCommand(mockCommand.Object);

            var helpCommand = new HelpCommand(registry);

            using var consoleOutput = new ConsoleOutput();
            helpCommand.Execute(Array.Empty<string>());

            Assert.Contains("Available commands:", consoleOutput.GetOutput());
            Assert.Contains(" - test", consoleOutput.GetOutput());
        }
    }

    public class ExitCommandTests
    {
        [Fact]
        public void Execute_ExitsApplication()
        {
            var exitCommand = new ExitCommand();

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                using var consoleOutput = new ConsoleOutput();
                exitCommand.Execute(Array.Empty<string>());
            });

            Assert.Equal("Exiting shell...", exception.Message);
        }
    }

    public class OutputHandlerTests
    {
        [Fact]
        public void Print_WritesMessageToConsole()
        {
            var outputHandler = new OutputHandler();

            using var consoleOutput = new ConsoleOutput();
            outputHandler.Print("Test message");

            Assert.Equal("Test message" + Environment.NewLine, consoleOutput.GetOutput());
        }
    }

    // Helper class to capture console output
    public class ConsoleOutput : IDisposable
    {
        private readonly System.IO.StringWriter _stringWriter;
        private readonly System.IO.TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new System.IO.StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput() => _stringWriter.ToString();

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}