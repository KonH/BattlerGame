using System.Collections.Generic;
using GameLogics.Client.Services;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public sealed class ItemsControl : MonoBehaviour {
		public Button Button;

		ClientStateService     _service;
		GameStateUpdateService _update;
		ItemService            _items;
		ItemsWindow.Factory    _windowFactory;

		ItemsWindow _window = null;

		[Inject]
		public void Init(ClientStateService service, GameStateUpdateService update, ItemService items, ItemsWindow.Factory windowFactory) {
			_service       = service;
			_update        = update;
			_items         = items;
			_windowFactory = windowFactory;
			Button.onClick.AddListener(OnClick);

			_update.OnStateUpdated += OnStateUpdated;
		}

		void OnDestroy() {
			_update.OnStateUpdated -= OnStateUpdated;
		}

		void OnStateUpdated(GameState _) {
			if ( !_window ) {
				return;
			}
			_window.Refresh(CollectItems());
		}

		void OnClick() {
			var items = CollectItems();
			_window = _windowFactory.Create(items);
		}

		ClickAction<ItemModel> Upgrade(ItemState item) {
			if ( !_items.HasUpgrade(item) ) {
				return null;
			}
			return new ClickAction<ItemModel>($"Upgrade ({_items.GetUpgradePriceStr(item)})",
				it => {
					_items.Upgrade((it as StateItemModel).State.Id);
				},
				_items.CanUpgrade(item.Id)
			);
		}

		List<ItemModel> CollectItems() {
			var result = new List<ItemModel>();
			foreach ( var state in _service.State.Items.Values ) {
				var config = _service.Config.Items[state.Descriptor];
				result.Add(new StateItemModel(state, config, Upgrade(state)));
			}
			return result;
		}
	}
}