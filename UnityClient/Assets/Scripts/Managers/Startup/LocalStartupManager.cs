using Zenject;

namespace UnityClient.Managers.Startup {
	public class LocalStartupManager : IInitializable {
		readonly GameSceneManager _sceneManager;

		public LocalStartupManager(GameSceneManager sceneManager) {
			_sceneManager = sceneManager;
		}
		
		public void Initialize() {
			_sceneManager.GoToWorld();
		}
	}
}