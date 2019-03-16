using System;
using GameLogics.Client.Services;
using GameLogics.Shared.Services;
using UnityClient.Managers;
using UnityClient.Services;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public ServerSettings ServerSettings;
		
		public override void InstallBindings() {
			Container.BindInstance(ServerSettings);
			BindServices();
			BindManagers();
			BindApiService();
		}

		void BindServices() {
			Container.Bind<ConvertService>().AsSingle();
			Container.Bind<INetworkService>().To<WebRequestNetworkService>().AsSingle();
			Container.Bind<ClientStateService>().AsSingle();
			Container.Bind<GameStateUpdateService>().AsSingle();
			Container.Bind<AuthService>().AsSingle();
			Container.Bind<RegisterService>().AsSingle();
		}

		void BindManagers() {
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.Bind<MainThreadRunner>().FromNewComponentOnRoot().AsSingle();
			Container.Bind<GameSceneManager>().AsSingle();
			Container.Bind(typeof(StartupManager), typeof(IInitializable), typeof(ITickable)).To<StartupManager>().AsSingle().NonLazy();
		}

		void BindApiService() {
			switch ( ServerSettings.Mode ) {
				case ServerMode.Network: Container.BindRemoteService(); break;
				case ServerMode.MemoryEmbedded: Container.BindEmbeddedService(true); break;
				case ServerMode.FileEmbedded: Container.BindEmbeddedService(false); break;
				default: throw new InvalidOperationException("Unknown server mode");
			}
		}
	}
}
