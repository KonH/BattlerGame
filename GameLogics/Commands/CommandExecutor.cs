using System;
using GameLogics.Core;

namespace GameLogics.Commands {
	public sealed class CommandExecutor {
		public event Action<GameState> OnStateUpdated = delegate {};

		GameState _state;
		
		public CommandExecutor(GameState state) {
			_state = state;
		}
		
		public void Execute(ICommand command) {
			command.Execute(_state);
			OnStateUpdated(_state);
		}
	}
}