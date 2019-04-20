using System;
using UnityEngine;
using UnityClient.Manager;
using UnityClient.Model;
using UnityClient.Service;
using UnityClient.View;
using UnityClient.ViewModel;
using Zenject;

namespace UnityClient.Installer {
	public sealed class LevelInstaller : MonoInstaller {
		public LevelManager  Manager;
		public UnitViewModel UnitViewModelPrefab;
		public UnitView      UnitViewPrefab;
		
		public override void InstallBindings() {
			Container.Bind<LevelService>().AsSingle();
			Container.Bind(typeof(LevelManager), typeof(IInitializable), typeof(IDisposable)).To<LevelManager>().FromInstance(Manager);
			Container.BindFactory<Transform, UnitLevelModel, UnitView, UnitViewModel, UnitViewModel.Factory>().FromComponentInNewPrefab(UnitViewModelPrefab);
			Container.BindFactory<UnitView, UnitView.Factory>().FromComponentInNewPrefab(UnitViewPrefab);
		}
	}
}