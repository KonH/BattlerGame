using System;
using System.Threading.Tasks;
using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;

namespace ConsoleClient {
	class Program {
		static void Main(string[] args) {
			AddResourceCase().GetAwaiter().GetResult();
		}

		static async Task AddResourceCase() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			var intentMapper = new HttpIntentToCommandMapper(stateManager, "http://localhost:8080/api/intent");
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