﻿using System;
using GameLogics.Client.Service;
using GameLogics.Server.Service;
using GameLogics.Shared.Service.ErrorHandle;
using GameLogics.Shared.Service;
using GameLogics.Shared.Service.Time;
using UnityClient.Manager;
using UnityClient.Service;
using Zenject;
using AuthService = GameLogics.Client.Service.AuthService;
using RegisterService = GameLogics.Client.Service.RegisterService;

namespace UnityClient.Installer {
	public sealed class CommonInstaller : MonoInstaller {
		public ServerSettings ServerSettings;
		
		public override void InstallBindings() {
			Container.BindInstance(ServerSettings);
			BindServices();
			BindManagers();
			BindApiService();
		}

		void BindServices() {
			Container.BindInstance(new EnvironmentService { IsDebugMode = ServerSettings.IsDebugMode });
			Container.Bind<ITimeService>().To<RealTimeService>().AsSingle();
			Container.Bind<OffsetTimeService>().ToSelf().AsSingle();
			Container.Bind<ConvertService>().AsSingle();
			Container.Bind<INetworkService>().To<WebRequestNetworkService>().AsSingle();
			Container.Bind<ClientStateService>().AsSingle();
			Container.Bind<GameStateUpdateService>().AsSingle();
			Container.Bind<MainThreadRunner>().FromNewComponentOnRoot().AsSingle();
			Container.Bind<ClientCommandRunner>().ToSelf().AsSingle();
			Container.Bind<AuthService>().AsSingle();
			Container.Bind<RegisterService>().AsSingle();
			Container.Bind<NoticeService>().AsSingle();
			Container.Bind<ApiErrorManager>().AsSingle().NonLazy();
			Container.Bind<ItemService>().AsSingle();
			Container.Bind<UnitService>().AsSingle();
		}

		void BindManagers() {
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			Container.Bind<GameSceneManager>().AsSingle();
			Container.Bind<IErrorHandleStrategy>().To<ReloadErrorHandleStrategy>().AsSingle();
			Container.Bind(typeof(StartupManager), typeof(ITickable)).To<StartupManager>().AsSingle().NonLazy();
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
