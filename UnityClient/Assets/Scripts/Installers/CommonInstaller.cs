using GameLogics.Commands;
using GameLogics.Core;
using Zenject;

namespace UnityClient.Installers {
	public class CommonInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.BindInstance(new GameState()).AsSingle();
			Container.Bind<CommandExecutor>().ToSelf().AsSingle();
		}
	}
}
