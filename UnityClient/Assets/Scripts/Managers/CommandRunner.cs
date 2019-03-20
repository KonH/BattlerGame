using System.Collections;
using System.Collections.Generic;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using UnityEngine;

namespace UnityClient.Managers {
	public class CommandRunner {
		public GameStateUpdateService Updater { get; }
		
		readonly MainThreadRunner       _runner;

		bool _hasRunningCommand = false;
		
		public CommandRunner(MainThreadRunner runner, GameStateUpdateService updateService) {
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
