using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Managers.Network;
using GameLogics.Repositories.State;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class WorldInstaller : MonoInstaller {
		public override void InstallBindings() {
			var stateManager = new InMemoryGameStateRepository(new GameState());
			Container.Bind<IGameStateRepository>().FromInstance(stateManager).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.Bind<IIntentToCommandMapper>().FromMethod(CreateComandMapper).AsSingle();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
		}

		IIntentToCommandMapper CreateComandMapper(InjectContext context) {
			var container = context.Container;
			var settings = container.Resolve<ServerSettings>();
			switch ( settings.Mode ) {
				case ServerMode.Embedded:
					return new DirectIntentToCommandMapper();
				case ServerMode.Network:
					return new NetworkIntentToCommandMapper(container.Resolve<ICustomLogger>(), container.Resolve<INetworkManager>());
				default:
					return null;
			}
		}
	}
}