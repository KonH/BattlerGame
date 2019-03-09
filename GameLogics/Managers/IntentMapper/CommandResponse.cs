using System.Collections.Generic;
using GameLogics.Commands;

namespace GameLogics.Managers.IntentMapper {
	public class CommandResponse {
		public readonly bool Success;
		public List<ICommand> Commands;

		public CommandResponse(bool success, List<ICommand> commands) {
			Success  = success;
			Commands = commands;
		}
		
		public static CommandResponse Failed() => new CommandResponse(false, null);
		
		public static CommandResponse FromCommands(List<ICommand> commands) => new CommandResponse(true, commands);
	}
}