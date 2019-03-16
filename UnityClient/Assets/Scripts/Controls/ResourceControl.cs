using GameLogics.Client.Services;
using GameLogics.Shared.Intents;
using GameLogics.Shared.Models;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class ResourceControl : MonoBehaviour {
		public Resource Kind;
		public int      Amount;

		MainThreadRunner       _runner;
		GameStateUpdateService _service;
		
		[Inject]
		public void Init(MainThreadRunner runner, GameStateUpdateService service) {
			_runner  = runner;
			_service = service;
		}

		public void Execute() {
			_runner.Run(async () => { await _service.Update(new RequestResourceIntent(Kind, Amount)); });
		}
	}
}

