using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.Services;
using Zenject;

namespace UnityClient.Installers {
	public class LevelInstaller : MonoInstaller {
		public LevelManager Manager;
		public UnitViewModel UnitPrefab;
		
		public override void InstallBindings() {
			Container.Bind<LevelService>().AsSingle();
			Container.Bind(typeof(LevelManager), typeof(IInitializable)).To<LevelManager>().FromInstance(Manager);
			Container.BindFactory<UnitLevelModel, UnitViewModel, UnitViewModel.Factory>().FromComponentInNewPrefab(UnitPrefab);
		}
	}
}