using System.IO;
using GameLogics.Commands;
using GameLogics.Intents;
using GameLogics.Managers;
using UnityClient.Starters;
using UnityEngine;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public override void InstallBindings() {
			var savePath = Path.Combine(Application.persistentDataPath, "save.json");
			var stateManager = new LocalGameStateManager(savePath);
			Container.Bind<IGameStateManager>().FromInstance(stateManager).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
			Container.Bind<IntentToCommandMapper>().ToSelf().AsSingle();
			Container.Bind<GameStateUpdater>().ToSelf().AsSingle();
			Container.Bind<CommonStarter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}
