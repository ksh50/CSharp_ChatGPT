using System;
using System.Collections.Generic;

namespace InteractiveShell
{
    class Program
    {
        static void Main(string[] args)
        {
            Shell shell = new Shell();
            shell.Run();
        }
    }

    class Shell
    {
        private readonly CommandReader _reader;
        private readonly CommandParser _parser;
        private readonly CommandRegistry _registry;
        private readonly CommandExecutor _executor;
        private readonly OutputHandler _output;

        public Shell()
        {
            _reader = new CommandReader();
            _parser = new CommandParser();
            _registry = new CommandRegistry();
            _executor = new CommandExecutor(_registry);
            _output = new OutputHandler();

            // コマンド登録
            _registry.RegisterCommand(new HelpCommand(_registry));
            _registry.RegisterCommand(new ExitCommand());
        }

        public void Run()
        {
            _output.Print("Interactive Shell started. Type 'help' for commands.");
            while (true)
            {
                string input = _reader.ReadCommand();
                var (command, args) = _parser.Parse(input);
                _executor.Execute(command, args);
            }
        }
    }

    class CommandReader
    {
        public string ReadCommand()
        {
            Console.Write("> ");
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
    }

    class CommandParser
    {
        public (string, string[]) Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return ("", Array.Empty<string>());
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (parts[0], parts.Length > 1 ? parts[1..] : Array.Empty<string>());
        }
    }

    interface ICommand
    {
        string Name { get; }
        void Execute(string[] args);
    }

    class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> _commands = new();

        public void RegisterCommand(ICommand command)
        {
            _commands[command.Name] = command;
        }

        public bool HasCommand(string name) => _commands.ContainsKey(name);
        public ICommand? GetCommand(string name) => _commands.GetValueOrDefault(name);
    }

    class CommandExecutor
    {
        private readonly CommandRegistry _registry;

        public CommandExecutor(CommandRegistry registry)
        {
            _registry = registry;
        }

        public void Execute(string command, string[] args)
        {
            if (_registry.HasCommand(command))
            {
                _registry.GetCommand(command)?.Execute(args);
            }
            else
            {
                Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
            }
        }
    }

    class HelpCommand : ICommand
    {
        private readonly CommandRegistry _registry;
        public string Name => "help";

        public HelpCommand(CommandRegistry registry)
        {
            _registry = registry;
        }

        public void Execute(string[] args)
        {
            Console.WriteLine("Available commands:");
            foreach (var cmd in _registry.GetType().GetFields())
            {
                Console.WriteLine($" - {cmd.Name}");
            }
        }
    }

    class ExitCommand : ICommand
    {
        public string Name => "exit";

        public void Execute(string[] args)
        {
            Console.WriteLine("Exiting shell...");
            Environment.Exit(0);
        }
    }

    class OutputHandler
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
