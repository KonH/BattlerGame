using System.Collections.Generic;

namespace GameLogics.Shared.Commands.Base {
	class CommandBuffer : ICommandBuffer {
		List<BaseCommand> _commands = new List<BaseCommand>();

		public CommandBuffer(BaseCommand firstCommand) {
			_commands.Add(firstCommand);
		}

		public void AddCommand(BaseCommand cmd) {
			_commands.Add(cmd);
		}
			
		public BaseCommand TryGetAt(int index) {
			if ( index < _commands.Count ) {
				return _commands[index];
			}
			return null;
		}
	}
}