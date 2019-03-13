using System;
using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using GameLogics.Managers.Register;
using GameLogics.Models;
using GameLogics.Repositories;
using GameLogics.Repositories.State;

namespace ConsoleClient {
	class Program {
		static ICustomLogger        _logger         = new ConsoleLogger();
		static INetworkManager      _networkManager = new HttpClientNetworkManager(_logger, "http://localhost:8080/");
		static IGameStateRepository _stateRepo      = new InMemoryGameStateRepository();

		static UserRepository _userRepository = new UserRepository {
			CurrentUser = User.CreateWithPassword("newUser", "password", "newUserName", "user")
		};
		
		static void Main(string[] args) {
			Main().GetAwaiter().GetResult();
		}

		static async Task Main() {
			await RegisterCase();
			await LoginCase();
			await AddResourceCase();
		}
		
		static async Task RegisterCase() {
			var registerManager = new NetworkRegisterManager(_logger, _networkManager, _userRepository);
			var result = await registerManager.TryRegister();
			Console.WriteLine("RegisterCase: {0}", result);
		}
		
		static async Task LoginCase() {
			var authManager = new NetworkAuthManager(_logger, _networkManager, _userRepository, _stateRepo);
			var result = await authManager.TryLogin();
			Console.WriteLine("LoginCase: {0} ({1})", result, _networkManager.AuthToken);
		}
		
		static async Task AddResourceCase() {
			Console.WriteLine($"AddResourceCase: starting Coins: {GetCoinsCount()}");
			var intentMapper = new NetworkIntentToCommandMapper(_logger, _networkManager, _stateRepo);
			var executor = new CommandExecutor(_stateRepo);
			var response = await intentMapper.RequestCommandsFromIntent(_stateRepo.State, new RequestResourceIntent(Resource.Coins, 1));
			if ( !response.Success ) {
				Console.WriteLine("AddResourceCase: failed");
				return;
			}
			Console.WriteLine("AddResourceCase: result commands:");
			var commands = response.Commands;
			foreach ( var cmd in commands ) {
				Console.WriteLine(cmd);
				executor.Execute(cmd);
			}
			Console.WriteLine($"AddResourceCase: result Coins: {GetCoinsCount()}");
		}

		static string GetCoinsCount() {
			return _stateRepo.State.Resources.TryGetValue(Resource.Coins, out var coins) ? coins.ToString() : "none";
		}
	}
}