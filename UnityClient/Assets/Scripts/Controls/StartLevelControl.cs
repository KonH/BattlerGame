using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class StartLevelControl : MonoBehaviour {
		public string LevelDesc;

		ClientCommandRunner _runner;
		GameSceneManager    _scene;
		
		[Inject]
		public void Init(ClientCommandRunner runner, GameSceneManager scene) {
			_runner = runner;
			_scene  = scene;

			_runner.Updater.OnCommandApplied += OnCommand;
		}

		void OnDestroy() {
			if ( _runner != null ) {
				_runner.Updater.OnCommandApplied -= OnCommand;
			}
		}

		void OnCommand(ICommand command) {
			if ( command is StartLevelCommand ) {
				_scene.GoToLevel();
			}
		}

		public void Execute() {
			var playerUnits = new List<ulong>(_runner.Updater.State.Units.Keys);
			_runner.TryAddCommand(new StartLevelCommand(LevelDesc, playerUnits));
		}
	}
}

