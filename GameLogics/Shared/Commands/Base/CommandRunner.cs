using System.Collections;
using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	public class CommandRunner : IEnumerable<CommandWorkItem> {
		readonly ICommand  _command;
		readonly GameState _state;
		readonly Config    _config;
		
		public CommandRunner(ICommand command, GameState state, Config config) {
			_command = command;
			_state   = state;
			_config  = config;
		}

		public IEnumerator<CommandWorkItem> GetEnumerator() {
			return new CommandEnumerator(_command, _state, _config);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}

	class TestCommand : ICommand {
		public bool IsValid(GameState state, Config config) {
			return false;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
		}
	}
}