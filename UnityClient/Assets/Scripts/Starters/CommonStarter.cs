using GameLogics.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Starters {
	public class CommonStarter : MonoBehaviour {
		IGameStateManager _stateManager;
		
		[Inject]
		public void Init(IGameStateManager stateManager) {
			_stateManager = stateManager;
		}

		void Awake() {
			_stateManager.Load();
		}

		void OnApplicationQuit() {
			_stateManager.Save();
		}
	}
}
