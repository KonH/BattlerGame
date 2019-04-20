using GameLogics.Client.Service;
using GameLogics.Shared.Command.Base;

namespace UnityClient.Service {
	public sealed class ClientCommandRunner {
		public GameStateUpdateService Updater { get; }
		
		readonly MainThreadRunner _runner;

		bool _hasRunningCommand = false;
		
		public ClientCommandRunner(MainThreadRunner runner, GameStateUpdateService updateService) {
			_runner = runner;
			Updater = updateService;
		}

		public bool IsValid(ICommand command) {
			return Updater.IsValid(command);
		}

		public bool TryAddCommand(ICommand command) {
			if ( _hasRunningCommand ) {
				return false;
			}
			_runner.Run(async () => {
				_hasRunningCommand = true;
				await Updater.Update(command);
				_hasRunningCommand = false;
			});
			return true;
		}
	}
}
