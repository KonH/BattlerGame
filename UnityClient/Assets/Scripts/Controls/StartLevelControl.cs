using System.Collections.Generic;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class StartLevelControl : MonoBehaviour {
		public string LevelDesc;

		MainThreadRunner       _runner;
		GameStateUpdateService _service;
		GameSceneManager       _scene;
		
		[Inject]
		public void Init(MainThreadRunner runner, GameStateUpdateService service, GameSceneManager scene) {
			_runner  = runner;
			_service = service;
			_scene   = scene;
		}

		public void Execute() {
			var playerUnits = new List<string>(_service.State.Units.Keys);
			_runner.Run(async () => {
				await _service.Update(new StartLevelCommand(LevelDesc, playerUnits));
				_scene.GoToLevel();
			});
		}
	}
}

