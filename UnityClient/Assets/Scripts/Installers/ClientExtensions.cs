using GameLogics.Client.Services;
using GameLogics.Shared.Services;
using Zenject;

namespace UnityClient.Installers {
	public static class ClientExtensions {
		public static void BindRemoteService(this DiContainer self) {
			self.Bind<IApiService>().To<ClientApiService>().AsSingle();
		}
	}
}