using System;
using GameLogics.Core;
using GameLogics.Managers;

namespace GameLogics.Commands {
	public sealed class CommandExecutor {
		public event Action<GameState> OnStateUpdated = delegate {};

		GameState _state;
		
		public CommandExecutor(IGameStateManager stateManager) {
			_state = stateManager.State;
		}
		
		public void Execute(ICommand command) {
			command.Execute(_state);
			OnStateUpdated(_state);
		}
	}
}