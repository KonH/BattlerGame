using System;
using System.Collections;
using System.Collections.Generic;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	class CommandEnumerator : IEnumerator<CommandWorkItem> {
		public CommandWorkItem Current { get; private set; }

		object IEnumerator.Current => Current;

		readonly GameState     _state;
		readonly Config        _config;
		readonly CommandBuffer _buffer;

		int _position = -1;
			
		public CommandEnumerator(ICommand command, GameState state, Config config) {
			_state  = state;
			_config = config;
			_buffer = new CommandBuffer();
			_buffer.Add(command);
		}
			
		public bool MoveNext() {
			if ( (Current != null) && !Current.Executed ) {
				throw new InvalidOperationException($"Current command isn't executed through {nameof(CommandWorkItem)}!");
			}
			_position++;
			var command = _buffer[_position];
			if ( command == null ) {
				// No commands in buffer
				return false;
			}
			Current = new CommandWorkItem(command, _state, _config, _buffer);
			return true;
		}

		public void Reset() {
			throw new NotSupportedException();
		}

		public void Dispose() {}
	}
}