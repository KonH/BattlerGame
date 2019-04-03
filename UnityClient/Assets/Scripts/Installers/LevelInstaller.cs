using System;
using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels;
using Zenject;

namespace UnityClient.Installers {
	public class LevelInstaller : MonoInstaller {
		public LevelManager  Manager;
		public UnitViewModel UnitPrefab;
		
		public override void InstallBindings() {
			Container.Bind<LevelService>().AsSingle();
			Container.Bind(typeof(LevelManager), typeof(IInitializable), typeof(IDisposable)).To<LevelManager>().FromInstance(Manager);
			Container.BindFactory<UnitLevelModel, UnitViewModel, UnitViewModel.Factory>().FromComponentInNewPrefab(UnitPrefab);
		}
	}
}