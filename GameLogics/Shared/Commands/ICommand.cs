using GameLogics.Shared.Models;

namespace GameLogics.Shared.Commands {
	/// <summary>
	/// Command is a game state modification (server-side at first, send to client-side to update local game state)
	/// </summary>
	public interface ICommand {
		void Execute(GameState state);
	}
}