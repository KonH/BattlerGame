using System;
using GameLogics.Core;
using GameLogics.Commands;

namespace GameLogics.Managers {
	public sealed class CommandExecutor {
		public event Action<GameState> OnStateUpdated = delegate {};

		readonly GameState _state;
		
		public CommandExecutor(IGameStateManager stateManager) {
			_state = stateManager.State;
		}
		
		public void Execute(ICommand command) {
			command.Execute(_state);
			OnStateUpdated(_state);
		}
	}
}