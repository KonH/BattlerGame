using System;
using GameLogics.Models;
using GameLogics.Commands;
using GameLogics.Repositories.State;

namespace GameLogics.Managers {
	public sealed class CommandExecutor {
		public event Action<GameState> OnStateUpdated = delegate {};

		readonly IGameStateRepository _stateRepository;
		
		public CommandExecutor(IGameStateRepository stateRepository) {
			_stateRepository = stateRepository;
		}
		
		public void Execute(ICommand command) {
			var state = _stateRepository.State;
			command.Execute(state);
			OnStateUpdated(state);
		}
	}
}