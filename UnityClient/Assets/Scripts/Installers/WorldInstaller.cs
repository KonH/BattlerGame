using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class WorldInstaller : MonoInstaller {
		public override void InstallBindings() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			Container.Bind<IGameStateManager>().FromInstance(stateManager).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.Bind<IIntentToCommandMapper>().FromMethod(CreateComandMapper).AsSingle();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
		}

		IIntentToCommandMapper CreateComandMapper(InjectContext context) {
			var container = context.Container;
			var settings = container.Resolve<ServerSettings>();
			var stateManager = container.Resolve<IGameStateManager>();
			switch ( settings.Mode ) {
				case ServerMode.Embedded:
					return new DirectIntentToCommandMapper(stateManager);
				case ServerMode.Network:
					return new NetworkIntentToCommandMapper(container.Resolve<ICustomLogger>(), stateManager, container.Resolve<INetworkManager>());
				default:
					return null;
			}
		}
	}
}