using System.Collections.Generic;
using UnityClient.Models;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class UnitsWindow : BaseWindow {		
		public class Factory : PlaceholderFactory<List<UnitModel>, UnitsWindow> {}
		
		public Button    CloseButton;
		public Transform ItemsRoot;
		
		[Inject]
		public void Init(UnitFragment.Factory unitFragment, Canvas parent, List<UnitModel> units) {
			CloseButton.onClick.AddListener(Hide);
			foreach ( var unit in units ) {
				unitFragment.Create(ItemsRoot, unit);
			}
			
			ShowAt(parent);
		}
	}
}
