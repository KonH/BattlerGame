using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class FinishLevelControl : MonoBehaviour {
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
			_runner.Run(async () => {
				await _service.Update(new FinishLevelCommand());
				_scene.GoToWorld();
			});
		}
	}
}

