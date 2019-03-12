using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Managers.Network;
using UnityClient.Managers;
using UnityClient.Managers.Startup;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public ServerSettings ServerSettings;
		
		public override void InstallBindings() {
			Container.Bind<MainThreadRunner>().FromNewComponentOnRoot().AsSingle();
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.Bind<GameSceneManager>().ToSelf().AsSingle();
			if ( ServerSettings.Mode == ServerMode.Network ) {
				InstallNetworkManagers();
			} else {
				InstallLocalManagers();
			}
		}

		void InstallNetworkManagers() {
			Container.BindInstance(ServerSettings);
			Container.Bind<INetworkManager>().To<WebRequestNetworkManager>().AsSingle();
			Container.Bind<UserManager>().ToSelf().AsSingle();
			Container.Bind<IAuthManager>().To<NetworkAuthManager>().AsSingle();
			Container.Bind(
					typeof(NetworkStartupManager), typeof(IInitializable), typeof(ITickable))
				.To<NetworkStartupManager>().AsSingle().NonLazy();
		}

		void InstallLocalManagers() {
			Container.Bind<IAuthManager>().To<LocalAuthManager>().AsSingle();
			Container.Bind<LocalStartupManager>().AsSingle().NonLazy();
		}
	}
}
