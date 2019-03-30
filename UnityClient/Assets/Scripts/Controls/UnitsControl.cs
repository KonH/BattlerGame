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
	public class UnitsControl : MonoBehaviour {
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
			_ui.ShowWindow<UnitWindow>(w => w.Show(model));
		}
	}
}