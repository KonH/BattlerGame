using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers.IntentMapper;
using GameLogics.Models;
using GameLogics.Repositories.State;

namespace GameLogics.Managers {
	public class GameStateUpdater {
		readonly IIntentToCommandMapper _mapper;
		readonly CommandExecutor        _executor;
		readonly IGameStateRepository   _stateRepository;

		Task _currentTask = null;
		
		public GameStateUpdater(IIntentToCommandMapper mapper, CommandExecutor executor, IGameStateRepository stateRepository) {
			_mapper          = mapper;
			_executor        = executor;
			_stateRepository = stateRepository;
		}

		public async Task Update(IIntent intent) {
			var state = _stateRepository.State;
			var response = await _mapper.RequestCommandsFromIntent(state, intent);
			foreach ( var cmd in response.Commands ) {
				_executor.Execute(cmd);
			}
		}

		public void TryUpdate(IIntent intent) {
			if ( _currentTask != null ) {
				if ( _currentTask.IsCompleted || _currentTask.IsFaulted || _currentTask.IsCanceled ) {
					_currentTask = null;
				} else {
					return;
				}
			}
			_currentTask = Update(intent);
		}
	}
}