using System;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command.Base {
	public sealed class CommandWorkItem {
		readonly GameState      _state;
		readonly ConfigRoot     _config;
		readonly ICommandBuffer _buffer;
		
		public ICommand Command  { get; }
		public bool     Executed { get; private set; }

		public CommandWorkItem(ICommand command, GameState state, ConfigRoot config, ICommandBuffer buffer) {
			_state  = state;
			_config = config;
			_buffer = buffer;
			Command = command;
		}
		
		public bool IsValid() {
			if ( Executed ) {
				throw new InvalidOperationException("Already executed!");
			}
			return Command.IsValid(_state, _config);
		}

		public void Execute() {
			if ( Executed ) {
				throw new InvalidOperationException("Already executed!");
			}
			Command.Execute(_state, _config, _buffer);
			Executed = true;
		}
	}
}