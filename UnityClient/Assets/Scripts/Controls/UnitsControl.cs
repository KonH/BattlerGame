using System.Collections.Generic;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using UnityClient.Managers;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public class UnitsControl : MonoBehaviour {
		public Button Button;

		UiManager           _ui;
		ClientStateService  _service;
		ClientCommandRunner _runner;
		
		[Inject]
		public void Init(UiManager ui, ClientStateService service, ClientCommandRunner runner) {
			_ui      = ui;
			_service = service;
			_runner  = runner;
			Button.onClick.AddListener(OnClick);
		}

		void OnClick() {
			var units = CollectUnits();
			var prefab = _ui.GetFragmentTemplate<UnitFragment>();
			_ui.ShowWindow<UnitsWindow>(w => w.Show(units, prefab, "Equip", OnUnitClick));
		}

		List<UnitModel> CollectUnits() {
			var result = new List<UnitModel>();
			foreach ( var state in _service.State.Units.Values ) {
				result.Add(new UnitModel(true, state, _service.Config.Units[state.Descriptor]));
			}
			return result;
		}

		void OnUnitClick(UnitModel model) {
			_ui.ShowWindow<UnitWindow>(w => {
				var itemTemplate = _ui.GetFragmentTemplate<ItemFragment>();
				w.Show(_runner, _service.State, _service.Config, OnEquipItemRequest, model, itemTemplate);
			});
		}

		void OnEquipItemRequest(UnitModel unit, ItemModel item) {
			var items = CollectItemsForType(item.FakeType);
			_ui.ShowWindow<ItemsWindow>(w => w.Show(items, _ui.GetFragmentTemplate<ItemFragment>(), "Equip", it => OnEquipItem(unit, it)));
		}

		void OnEquipItem(UnitModel unit, ItemModel item) {
			_runner.TryAddCommand(new EquipItemCommand(item.State.Id, unit.State.Id));
		}

		List<ItemModel> CollectItemsForType(ItemType type) {
			var result = new List<ItemModel>();
			foreach ( var state in _service.State.Items.Values ) {
				var config = _service.Config.Items[state.Descriptor];
				if ( config.Type != type ) {
					continue;
				}
				result.Add(new ItemModel(state, config));
			}
			return result;
		}
	}
}