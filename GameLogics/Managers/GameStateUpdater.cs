using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers.IntentMapper;

namespace GameLogics.Managers {
	public class GameStateUpdater {
		readonly IIntentToCommandMapper _mapper;
		readonly CommandExecutor        _executor;

		Task _currentTask = null;
		
		public GameStateUpdater(IIntentToCommandMapper mapper, CommandExecutor executor) {
			_mapper   = mapper;
			_executor = executor;
		}

		public async Task Update(IIntent intent) {
			var response = await _mapper.RequestCommandsFromIntent(intent);
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