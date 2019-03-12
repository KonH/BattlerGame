using GameLogics.Managers;
using GameLogics.Managers.Network;
using GameLogics.Repositories;
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
			Container.BindInstance(ServerSettings);
			if ( ServerSettings.Mode == ServerMode.Network ) {
				InstallNetworkManagers();
			} else {
				InstallLocalManagers();
			}
		}

		void InstallNetworkManagers() {
			Container.Bind<INetworkManager>().To<WebRequestNetworkManager>().AsSingle();
			Container.Bind<UserRepository>().ToSelf().AsSingle();
			Container.Bind<NetworkAuthManager>().ToSelf().AsSingle();
			Container.Bind(typeof(NetworkStartupManager), typeof(IInitializable), typeof(ITickable))
				.To<NetworkStartupManager>().AsSingle().NonLazy();
		}

		void InstallLocalManagers() {
			Container.Bind(typeof(LocalStartupManager), typeof(IInitializable))
				.To<LocalStartupManager>().AsSingle().NonLazy();
		}
	}
}
