using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public override void InstallBindings() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			Container.Bind<IGameStateManager>().FromInstance(stateManager).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.BindInstance(new WebRequestIntentToCommandMapper.Settings { BaseUrl = "http://localhost:8080/" });
			Container.Bind<IIntentToCommandMapper>().To<WebRequestIntentToCommandMapper>().AsSingle();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
		}
	}
}
