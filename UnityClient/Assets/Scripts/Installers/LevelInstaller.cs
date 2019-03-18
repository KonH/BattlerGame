using UnityClient.Managers;
using Zenject;

namespace UnityClient.Installers {
	public class LevelInstaller : MonoInstaller {
		public LevelManager Manager;
		public UnitViewModel UnitPrefab;
		
		public override void InstallBindings() {
			Container.Bind(typeof(LevelManager), typeof(IInitializable)).To<LevelManager>().FromInstance(Manager);
			Container.BindFactory<UnitViewModel, UnitViewModel.Factory>().FromComponentInNewPrefab(UnitPrefab);
		}
	}
}