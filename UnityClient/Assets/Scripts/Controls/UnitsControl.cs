using System.Collections.Generic;
using GameLogics.Client.Services;
using UnityClient.Models;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public sealed class UnitsControl : MonoBehaviour {
		public Button Button;

		ClientStateService  _service;

		UnitsWindow.Factory _unitsWindow;
		UnitWindow.Factory _unitWindow;

		ItemsWindow _equipWindow;
		
		[Inject]
		public void Init(ClientStateService service, UnitsWindow.Factory unitsWindow, UnitWindow.Factory unitWindow) {
			_service     = service;
			_unitsWindow = unitsWindow;
			_unitWindow  = unitWindow;
			
			Button.onClick.AddListener(OnClick);
		}

		void OnClick() {
			var units = CollectUnits();
			_unitsWindow.Create(units);
		}

		List<UnitModel> CollectUnits() {
			var result = new List<UnitModel>();
			foreach ( var state in _service.State.Units.Values ) {
				result.Add(new StateUnitModel(state, 0, new ClickAction<UnitModel>("Equip", u => {
					_unitWindow.Create((StateUnitModel)u);
				})));
			}
			return result;
		}
	}
}