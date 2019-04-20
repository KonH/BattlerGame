using System.Collections.Generic;
using UnityClient.Model;
using UnityClient.ViewModel.Fragment;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModel.Window {
	public sealed class ItemsWindow : BaseWindow {		
		public sealed class Factory : PlaceholderFactory<List<ItemModel>, ItemsWindow> {}
		
		public Button    CloseButton;
		public Transform ItemsRoot;

		ItemFragment.Factory _itemFragment;

		List<ItemFragment> _fragments = new List<ItemFragment>();

		[Inject]
		public void Init(Canvas parent, ItemFragment.Factory itemFragment, List<ItemModel> items) {
			_itemFragment = itemFragment;

			CloseButton.onClick.AddListener(Hide);
			Refresh(items);
			ShowAt(parent);
		}

		public void Refresh(List<ItemModel> items) {
			while ( items.Count < _fragments.Count ) {
				var lastIndex = _fragments.Count - 1;
				_fragments[lastIndex].gameObject.SetActive(false);
				_fragments.RemoveAt(lastIndex);
			}
			for ( var i = 0; i < items.Count; i++ ) {
				var item = items[i];
				if ( i >= _fragments.Count ) {
					_fragments.Add(_itemFragment.Create(ItemsRoot, item));
				} else {
					_fragments[i].Refresh(item);
				}
			}
		}
	}
}
