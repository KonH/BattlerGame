using System.IO;
using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using GameLogics.Managers.Register;
using GameLogics.Repositories;
using GameLogics.Repositories.State;
using UnityClient.Managers;
using UnityEngine;
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
			Container.Bind(typeof(StartupManager), typeof(IInitializable), typeof(ITickable)).To<StartupManager>().AsSingle().NonLazy();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
			
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
			Container.Bind<IGameStateRepository>().To<InMemoryGameStateRepository>().AsSingle();
			Container.Bind<IIntentToCommandMapper>().To<NetworkIntentToCommandMapper>().AsSingle();
		}

		void InstallLocalManagers() {
			Container.Bind<IAuthManager>().To<LocalAuthManager>().AsSingle();
			Container.Bind<IRegisterManager>().To<LocalRegisterManager>().AsSingle();
			var statePath = Path.Combine(Application.persistentDataPath, ServerSettings.LocalStateFileName);
			Container.Bind(typeof(IGameStateRepository), typeof(LocalGameStateRepository)).FromInstance(new LocalGameStateRepository(statePath)).AsSingle();
			Container.Bind(typeof(LocalGameStateManager), typeof(IInitializable)).To<LocalGameStateManager>().AsSingle();
			Container.Bind<IIntentToCommandMapper>().To<DirectIntentToCommandMapper>().AsSingle();
		}
	}
}
