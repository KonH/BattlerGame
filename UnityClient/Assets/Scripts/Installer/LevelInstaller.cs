using System;
using UnityClient.Manager;
using UnityClient.Model;
using UnityClient.Service;
using UnityClient.ViewModel;
using Zenject;

namespace UnityClient.Installer {
	public sealed class LevelInstaller : MonoInstaller {
		public LevelManager  Manager;
		public UnitViewModel UnitPrefab;
		
		public override void InstallBindings() {
			Container.Bind<LevelService>().AsSingle();
			Container.Bind(typeof(LevelManager), typeof(IInitializable), typeof(IDisposable)).To<LevelManager>().FromInstance(Manager);
			Container.BindFactory<UnitLevelModel, UnitViewModel, UnitViewModel.Factory>().FromComponentInNewPrefab(UnitPrefab);
		}
	}
}