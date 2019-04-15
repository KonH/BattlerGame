using System;
using System.Collections.Generic;
using UnityClient.Manager;
using UnityClient.Model;
using UnityClient.ViewModel.Window;
using UnityEngine;
using Zenject;

namespace UnityClient.Installer {
	public sealed class WindowInstaller : MonoInstaller {
		[Header("Windows")]
		public NoticeWindow     NoticeWindowPrefab;
		public WinWindow        WinWindowPrefab;
		public LoseWindow       LoseWindowPrefab;
		public UnitWindow       UnitWindowPrefab;
		public UnitsWindow      UnitsWindowPrefab;
		public ItemsWindow      ItemsWindowPrefab;
		public StartLevelWindow StartLevelWindowPrefab;
		
		[Header("Scene")]
		public Canvas UICanvas;
		
		public override void InstallBindings() {
			Container.BindInstance(UICanvas);
			Container.BindFactory<NoticeModel, NoticeWindow, NoticeWindow.Factory>().FromComponentInNewPrefab(NoticeWindowPrefab);
			Container.BindFactory<Action, WinWindow, WinWindow.Factory>().FromComponentInNewPrefab(WinWindowPrefab);
			Container.BindFactory<Action, LoseWindow, LoseWindow.Factory>().FromComponentInNewPrefab(LoseWindowPrefab);
			Container.BindFactory<StateUnitModel, UnitWindow, UnitWindow.Factory>().FromComponentInNewPrefab(UnitWindowPrefab);
			Container.BindFactory<List<UnitModel>, UnitsWindow, UnitsWindow.Factory>().FromComponentInNewPrefab(UnitsWindowPrefab);
			Container.BindFactory<List<ItemModel>, ItemsWindow, ItemsWindow.Factory>().FromComponentInNewPrefab(ItemsWindowPrefab);
			Container.BindFactory<string, Action, StartLevelWindow, StartLevelWindow.Factory>().FromComponentInNewPrefab(StartLevelWindowPrefab);

			Container.Bind(typeof(NoticeManager), typeof(ITickable)).To<NoticeManager>().AsSingle().NonLazy();
		}
	}
}
