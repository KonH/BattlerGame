using System.Collections.Generic;
using UnityClient.Model;
using UnityClient.ViewModel.Fragment;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModel.Window {
	public sealed class UnitsWindow : BaseWindow {		
		public sealed class Factory : PlaceholderFactory<List<UnitModel>, UnitsWindow> {}
		
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
