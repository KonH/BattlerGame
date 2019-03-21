using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	/// <summary>
	/// Base interface for commands, allow only get internal set of pending commands
	/// ExecuteCommand is required to proceed to next command
	/// </summary>
	public interface ICompositeCommand : ICommand {
		bool IsFirstCommandValid(GameState state, Config config);
		IEnumerable<IPendingCommand> AsEnumerable();
	}
}