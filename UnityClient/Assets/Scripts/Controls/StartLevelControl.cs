using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using UnityClient.Managers;
using UnityClient.Services;
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

			_runner.Updater.AddHandler<StartLevelCommand>(OnStartLevel);
		}

		void OnDestroy() {
			_runner?.Updater.RemoveHandler<StartLevelCommand>(OnStartLevel);
		}

		Task OnStartLevel(ICommand _) {
			_scene.GoToLevel();
			return Task.CompletedTask;
		}

		public void Execute() {
			var playerUnits = _runner.Updater.State.Units.Where(u => u.Value.Health > 0).Select(u => u.Key).ToList();
			_runner.TryAddCommand(new StartLevelCommand(LevelDesc, playerUnits));
		}
	}
}

