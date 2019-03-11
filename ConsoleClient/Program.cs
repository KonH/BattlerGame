using System;
using System.Threading.Tasks;
using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using GameLogics.Models;

namespace ConsoleClient {
	class Program {
		
		static ICustomLogger   _logger         = new ConsoleLogger();
		static INetworkManager _networkManager = new HttpClientNetworkManager(_logger, "http://localhost:8080/");
		
		static void Main(string[] args) {
			Main().GetAwaiter().GetResult();
		}

		static async Task Main() {
			await AddResourceCase();
			await RegisterCase();
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

		static async Task RegisterCase() {
			var registerManager = new RegisterManager(_logger, _networkManager);
			var user = User.CreateWithPassword("newUser", "password", "newUser", "user");
			var result = await registerManager.TryRegister(user);
			Console.WriteLine("RegisterCase: {0}", result);
		}
	}
}