using System;
using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public enum ServerMode {
			Embedded,
			Network
		}

		public ServerMode                        ActiveServerMode;
		public WebRequestNetworkManager.Settings ServerSettings;
		
		public override void InstallBindings() {
			Container.Bind<ICustomLogger>().To<UnityLogger>().AsSingle();
			var stateManager = new InMemoryGameStateManager(new GameState());
			Container.Bind<IGameStateManager>().FromInstance(stateManager).AsSingle();
			Container.BindInstance(ServerSettings);
			Container.Bind<INetworkManager>().To<WebRequestNetworkManager>().AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			BindIntentToCommandMapper();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
		}

		void BindIntentToCommandMapper() {
			Container.Bind<IIntentToCommandMapper>().To(GetIntentToCommandMapperType()).AsSingle();
		}

		Type GetIntentToCommandMapperType() {
			switch ( ActiveServerMode ) {
				case ServerMode.Embedded: return typeof(DirectIntentToCommandMapper);
				case ServerMode.Network : return typeof(NetworkIntentToCommandMapper);
				default: return null;
			}
		}
	}
}
