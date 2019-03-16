using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	/// <summary>
	/// Command is a game state modification (server-side at first, send to client-side to update local game state)
	/// </summary>
	public interface ICommand {
		bool IsValid(GameState state, Config config);
		void Execute(GameState state, Config config);
	}
}