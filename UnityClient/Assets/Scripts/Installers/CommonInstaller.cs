using System;
using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public enum ServerMode {
			Embedded,
			Network
		}

		public ServerMode                               ActiveServerMode;
		public WebRequestIntentToCommandMapper.Settings ServerSettings;
		
		public override void InstallBindings() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			Container.Bind<IGameStateManager>().FromInstance(stateManager).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.BindInstance(ServerSettings);
			BindIntentToCommandMapper();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
		}

		void BindIntentToCommandMapper() {
			Container.Bind<IIntentToCommandMapper>().To(GetIntentToCommandMapperType()).AsSingle();
		}

		Type GetIntentToCommandMapperType() {
			switch ( ActiveServerMode ) {
				case ServerMode.Embedded: return typeof(DirectIntentToCommandMapper);
				case ServerMode.Network : return typeof(WebRequestIntentToCommandMapper);
				default: return null;
			}
		}
	}
}
