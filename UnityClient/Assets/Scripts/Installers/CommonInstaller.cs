using GameLogics.Managers;
using GameLogics.Managers.Auth;
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
			Container.Bind<IAuthManager>().FromMethod(CreateAuthManager).AsSingle();
		}
		
		IAuthManager CreateAuthManager(InjectContext context) {
			var container = context.Container;
			var settings  = container.Resolve<ServerSettings>();
			switch ( settings.Mode ) {
				case ServerMode.Embedded:
					return new LocalAuthManager();
				case ServerMode.Network:
					return new NetworkAuthManager(container.Resolve<ICustomLogger>(), container.Resolve<INetworkManager>());
				default:
					return null;
			}
		}
	}
}
