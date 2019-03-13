using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Models;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class ResourceControl : MonoBehaviour {
		public Resource Kind;
		public int      Amount;

		GameStateUpdater _updater;
		
		[Inject]
		public void Init(GameStateUpdater updater) {
			_updater = updater;
		}

		public void Execute() {
			_updater.TryUpdate(new RequestResourceIntent(Kind, Amount));
		}
	}
}

