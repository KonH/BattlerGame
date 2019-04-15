using System.Collections;
using System.Collections.Generic;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command.Base {
	public sealed class CommandRunner : IEnumerable<CommandWorkItem> {
		readonly ICommand   _command;
		readonly GameState  _state;
		readonly ConfigRoot _config;
		
		public CommandRunner(ICommand command, GameState state, ConfigRoot config) {
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