using System;
using System.Threading.Tasks;
using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;

namespace ConsoleClient {
	class Program {
		static void Main(string[] args) {
			Main().GetAwaiter().GetResult();
		}

		static async Task Main() {
			await AddResourceCase();
		}
		
		static async Task AddResourceCase() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			var networkManager = new HttpClientNetworkManager("http://localhost:8080/");
			var intentMapper = new NetworkIntentToCommandMapper(stateManager, networkManager);
			var response = await intentMapper.RequestCommandsFromIntent(new RequestResourceIntent(Resource.Coins, 1));
			if ( !response.Success ) {
				return;
			}
			var commands = response.Commands;
			foreach ( var cmd in commands ) {
				Console.WriteLine(cmd);
			}
		}
	}
}