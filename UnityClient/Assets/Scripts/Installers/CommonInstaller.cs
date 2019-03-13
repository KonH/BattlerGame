using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Managers.Network;
using GameLogics.Managers.Register;
using GameLogics.Repositories;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public ServerSettings ServerSettings;
		
		public override void InstallBindings() {
			Container.Bind<MainThreadRunner>().FromNewComponentOnRoot().AsSingle();
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.Bind<GameSceneManager>().ToSelf().AsSingle();
			Container.BindInstance(ServerSettings);
			Container.Bind<UserRepository>().ToSelf().AsSingle();
			Container.Bind(typeof(StartupManager), typeof(IInitializable), typeof(ITickable))
				.To<StartupManager>().AsSingle().NonLazy();
			
			if ( ServerSettings.Mode == ServerMode.Network ) {
				InstallNetworkManagers();
			} else {
				InstallLocalManagers();
			}
		}

		void InstallNetworkManagers() {
			Container.Bind<INetworkManager>().To<WebRequestNetworkManager>().AsSingle();
			Container.Bind<IAuthManager>().To<NetworkAuthManager>().AsSingle();
			Container.Bind<IRegisterManager>().To<NetworkRegisterManager>().AsSingle();
		}

		void InstallLocalManagers() {
			Container.Bind<IAuthManager>().To<LocalAuthManager>().AsSingle();
			Container.Bind<IRegisterManager>().To<LocalRegisterManager>().AsSingle();
		}
	}
}
