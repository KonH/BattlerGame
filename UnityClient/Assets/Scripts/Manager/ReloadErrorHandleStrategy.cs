using GameLogics.Client.Service;
using GameLogics.Shared.Service.ErrorHandle;
using GameLogics.Shared.Dao.Api.Error;

namespace UnityClient.Manager {
	public sealed class ReloadErrorHandleStrategy : IErrorHandleStrategy {
		readonly GameSceneManager   _scene;
		readonly ClientStateService _state;

		public ReloadErrorHandleStrategy(GameSceneManager scene, ClientStateService state) {
			_scene = scene;
			_state = state;
		}
		
		public void OnError(IApiError error) {
			if ( _scene.IsLoginOrRegister ) {
				return;
			}
			_scene.GoToStart();
			_state.User  = null;
			_state.State = null;
		}
	}
}