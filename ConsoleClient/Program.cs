using System;
using System.Threading.Tasks;
using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using GameLogics.Models;

namespace ConsoleClient {
	class Program {
		static ICustomLogger   _logger         = new ConsoleLogger();
		static INetworkManager _networkManager = new HttpClientNetworkManager(_logger, "http://localhost:8080/");

		static UserManager _userManager = new UserManager {
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
			var registerManager = new RegisterManager(_logger, _networkManager, _userManager);
			var result = await registerManager.TryRegister();
			Console.WriteLine("RegisterCase: {0}", result);
		}
		
		static async Task LoginCase() {
			var authManager = new NetworkAuthManager(_logger, _networkManager, _userManager);
			var result = await authManager.TryLogin();
			Console.WriteLine("LoginCase: {0} ({1})", result, _networkManager.AuthToken);
		}
		
		static async Task AddResourceCase() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			var intentMapper = new NetworkIntentToCommandMapper(_logger, stateManager, _networkManager);
			var response = await intentMapper.RequestCommandsFromIntent(new RequestResourceIntent(Resource.Coins, 1));
			if ( !response.Success ) {
				Console.WriteLine("AddResourceCase: failed");
				return;
			}
			Console.WriteLine("AddResourceCase: result commands:");
			var commands = response.Commands;
			foreach ( var cmd in commands ) {
				Console.WriteLine(cmd);
			}
		}
	}
}