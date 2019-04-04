using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GameLogics.Client.Models;
using GameLogics.Shared.Commands.Base;

namespace ConsoleClient {
	public static class EntryPoint {
		static List<(Type, ConstructorInfo)> _commands = CreateCommandMap();
		
		static Client _client;

		public static void Run() {
			Console.WriteLine("[ConsoleClient]");
			Console.WriteLine("Select mode:");
			Console.WriteLine("1) Local");
			Console.WriteLine("2) Network");
			var mode = Input.ReadScopedInt("Mode", 1, 2);
			_client = CreateClient(mode);

			Console.WriteLine("Select action:");
			Console.WriteLine("1) Register");
			Console.WriteLine("2) Login");
			var action = Input.ReadScopedInt("Action", 1, 2);
			if ( action == 1 ) {
				Register();
			}
			Login();
			
			DrawConfig();
			
			ProcessCommands();
		}

		static Client CreateClient(int mode) {
			switch ( mode ) {
				case 1: return new Client().AddServerApiService();
				case 2: return new Client().AddClientApiService();
				default: throw new InvalidOperationException();
			}
		}

		static void Register() {
			Console.WriteLine("Register:");
			var name     = Input.ReadString("Name");
			var password = Input.ReadString("Password");
			var newUser  = new User(name, password, name);
			
			Console.WriteLine("Register...");
			var result = _client.Register.TryRegister(newUser).WaitForResult();
			ConsoleLogger.WriteWithColor(
				result ? ConsoleColor.Green : ConsoleColor.Red,
				$"Is user registered: {result}"
			);
		}

		static void Login() {
			Console.WriteLine("Login:");
			var name     = Input.ReadString("Name");
			var password = Input.ReadString("Password");
			var user     = new User(name, password, name);

			Console.WriteLine("Login...");
			var result = _client.Auth.TryLogin(user).WaitForResult();
			ConsoleLogger.WriteWithColor(
				result ? ConsoleColor.Green : ConsoleColor.Red,
				$"Is login success: {result}"
			);
		}

		static void ProcessCommands() {
			Console.WriteLine("Ready to process commands.");
			while ( true ) {
				DrawState();
				DrawCommands();
				var selectedIndex = Input.ReadScopedInt("Command (0 for exit)", 0, _commands.Count);
				if ( selectedIndex == 0 ) {
					return;
				}
				var cmd = CreateCommand(selectedIndex - 1);
				ExecuteCommand(cmd);
			}
		}

		static void DrawConfig() {
			Console.WriteLine("Current config:");
			ConsoleLogger.WriteWithColor(ConsoleColor.Gray, _client.Convert.ToJson(_client.State.Config));
		}
		
		static void DrawState() {
			Console.WriteLine("Current state:");
			ConsoleLogger.WriteWithColor(ConsoleColor.Gray, _client.Convert.ToJson(_client.State.State));
		}

		static void DrawCommands() {
			Console.WriteLine("Available commands:");
			for ( var i = 0; i < _commands.Count; i++ ) {
				Console.WriteLine($"{i + 1}) {_commands[i].Item1.Name}");
			}
		}
		
		static ICommand CreateCommand(int index) {
			var (type, constructor) = _commands[index];
			Console.WriteLine($"Selected command: {type.Name}");
			var args = CollectCommandArgs(constructor);
			var instance = Activator.CreateInstance(type, args);
			Console.WriteLine($"Created command: {instance}");
			return instance as ICommand;
		}

		static object[] CollectCommandArgs(ConstructorInfo constructor) {
			var parameters = constructor.GetParameters();
			var args = new object[parameters.Length];
			if ( args.Length > 0 ) {
				Console.WriteLine("Arguments:");
			}
			for ( var i = 0; i < args.Length; i++ ) {
				var param = parameters[i];
				var value = Input.ReadArbitraryValue($"{param.Name} ({param.ParameterType})", param.ParameterType);
				args[i] = value;
			}
			return args;
		}

		static void ExecuteCommand(ICommand command) {
			var state  = _client.State.State;
			var config = _client.State.Config;
			var isValid = command.IsValid(state, config);
			ConsoleLogger.WriteWithColor(
				isValid ? ConsoleColor.Green : ConsoleColor.Red,
				$"IsCommandValid: {isValid}"
			);
			if ( !isValid ) {
				return;
			}
			_client.Updater.Update(command).WaitForResult();
		}

		static List<(Type, ConstructorInfo)> CreateCommandMap() {
			var result = new List<(Type, ConstructorInfo)>();
			var commands = GetCommandTypes();
			foreach ( var command in commands ) {
				result.Add((command, GetConstructor(command)));
			}
			return result;
		}
		
		static List<Type> GetCommandTypes() {
			var types    = typeof(ICommand).Assembly.GetTypes();
			var commands = new List<Type>();
			foreach ( var type in types ) {
				if ( type.IsAbstract ) {
					continue;
				}
				if ( !typeof(ICommand).IsAssignableFrom(type) ) {
					continue;
				}
				if ( typeof(IInternalCommand).IsAssignableFrom(type) ) {
					continue;
				}
				commands.Add(type);
			}
			return commands;
		}

		static ConstructorInfo GetConstructor(Type type) {
			var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
			if ( constructors.Length > 1 ) {
				throw new InvalidOperationException($"More than one public constructor for type {type}");
			}
			return constructors[0];
		}
		
		static T WaitForResult<T>(this Task<T> task) => task.GetAwaiter().GetResult();
		
		static void WaitForResult(this Task task) => task.GetAwaiter().GetResult();
	}
}