using System.Collections.Generic;
using GameLogics.Client.Services;
using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.ViewModels.Fragments;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public class ItemsControl : MonoBehaviour {
		public Button Button;

		UiManager         _ui;
		ClientStateService _service;
		
		[Inject]
		public void Init(UiManager ui, ClientStateService service) {
			_ui      = ui;
			_service = service;
			Button.onClick.AddListener(OnClick);
		}

		void OnClick() {
			var items = CollectItems();
			var prefab = _ui.GetFragmentTemplate<ItemFragment>();
			_ui.ShowWindow<ItemsWindow>(w => w.Show(items, prefab));
		}

		List<ItemModel> CollectItems() {
			var result = new List<ItemModel>();
			foreach ( var state in _service.State.Items.Values ) {
				result.Add(new ItemModel(state, _service.Config.Items[state.Descriptor]));
			}
			return result;
		}
	}
}