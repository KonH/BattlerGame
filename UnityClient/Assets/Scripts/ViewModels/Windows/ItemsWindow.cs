using System.Collections.Generic;
using UnityClient.Models;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class ItemsWindow : BaseWindow {		
		public class Factory : PlaceholderFactory<List<ItemModel>, ItemsWindow> {}
		
		public Button    CloseButton;
		public Transform ItemsRoot;

		[Inject]
		public void Init(Canvas parent, ItemFragment.Factory itemFragment, List<ItemModel> items) {
			CloseButton.onClick.AddListener(Hide);
			foreach ( var item in items ) {
				itemFragment.Create(ItemsRoot, item);
			}
			
			ShowAt(parent);
		}
	}
}
