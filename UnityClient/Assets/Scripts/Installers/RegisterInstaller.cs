using GameLogics.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class RegisterInstaller : MonoInstaller {
		public override void InstallBindings() {
			Container.Bind<RegisterManager>().ToSelf().AsSingle();
		}
	}
}