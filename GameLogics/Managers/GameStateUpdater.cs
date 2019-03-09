using GameLogics.Commands;
using GameLogics.Intents;

namespace GameLogics.Managers {
	public class GameStateUpdater {
		readonly IntentToCommandMapper _mapper;
		readonly CommandExecutor       _executor;

		public GameStateUpdater(IntentToCommandMapper mapper, CommandExecutor executor) {
			_mapper   = mapper;
			_executor = executor;
		}

		public void Update(IIntent intent) {
			var commands = _mapper.CreateCommandsFromIntent(intent);
			foreach ( var command in commands ) {
				_executor.Execute(command);
			}
		}
	}
}