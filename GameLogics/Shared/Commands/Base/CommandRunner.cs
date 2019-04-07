using System.Collections;
using System.Collections.Generic;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	public sealed class CommandRunner : IEnumerable<CommandWorkItem> {
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
}