using GameLogics.Managers;
using GameLogics.Managers.Network;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public ServerSettings ServerSettings;
		
		public override void InstallBindings() {
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.BindInstance(ServerSettings);
			Container.Bind<INetworkManager>().To<WebRequestNetworkManager>().AsSingle();
		}
	}
}
