using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	/// <summary>
	/// Command, which ready for execution
	/// </summary>
	public interface IPendingCommand : ICommand {
		bool IsCommandValid(GameState state, Config config);
		void ExecuteCommand(GameState state, Config config);
	}
}