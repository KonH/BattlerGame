using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;
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
			_updater.Update(new RequestResourceIntent(Kind, Amount));
		}
	}
}

