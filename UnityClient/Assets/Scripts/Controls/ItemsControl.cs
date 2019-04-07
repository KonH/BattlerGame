using System.Collections.Generic;
using GameLogics.Client.Services;
using UnityClient.Models;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public sealed class ItemsControl : MonoBehaviour {
		public Button Button;

		ClientStateService  _service;
		ItemsWindow.Factory _window;
		
		[Inject]
		public void Init(ClientStateService service, ItemsWindow.Factory window) {
			_service = service;
			_window  = window;
			Button.onClick.AddListener(OnClick);
		}

		void OnClick() {
			var items = CollectItems();
			_window.Create(items);
		}

		List<ItemModel> CollectItems() {
			var result = new List<ItemModel>();
			foreach ( var state in _service.State.Items.Values ) {
				var config = _service.Config.Items[state.Descriptor];
				result.Add(new StateItemModel(state, config, null));
			}
			return result;
		}
	}
}