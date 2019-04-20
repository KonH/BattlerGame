using GameLogics.Client.Service;
using GameLogics.Shared.Service;
using Zenject;

namespace UnityClient.Installer {
	public static class ClientExtensions {
		public static void BindRemoteService(this DiContainer self) {
			self.Bind<IApiService>().To<ClientApiService>().AsSingle();
		}
	}
}