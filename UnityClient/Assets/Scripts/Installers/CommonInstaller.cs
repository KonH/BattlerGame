using System;
using GameLogics.Client.Services;
using GameLogics.Client.Services.ErrorHandle;
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
			Container.Bind<NoticeService>().AsSingle();
			Container.Bind<ApiErrorManager>().AsSingle().NonLazy();
		}

		void BindManagers() {
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.Bind<MainThreadRunner>().FromNewComponentOnRoot().AsSingle();
			Container.Bind<GameSceneManager>().AsSingle();
			Container.Bind<IErrorHandleStrategy>().To<ReloadErrorHandleStrategy>().AsSingle();
			Container.Bind(typeof(StartupManager), typeof(ITickable)).To<StartupManager>().AsSingle().NonLazy();
			Container.Bind<ClientCommandRunner>().ToSelf().AsSingle();
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
