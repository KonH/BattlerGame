using System;
using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using TMPro;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class StartLevelWindow : BaseWindow {		
		public class Factory : PlaceholderFactory<string, Action, StartLevelWindow> {}

		public TMP_Text  Header;
		public Button    CloseButton;
		public Button    StartButton;
		public Transform ItemsRoot;

		GameStateUpdateService _update;
		UnitService            _units;
		UnitFragment.Factory   _unitFragment;
		UnitsWindow.Factory    _unitsWindow;
		UnitsWindow            _selectWindow;
		string                 _levelDesc;
		Action                 _callback;

		UnitModel[]    _selectedUnits     = null;
		UnitFragment[] _fragments         = null;
		int            _selectedUnitIndex = -1;
		
		[Inject]
		public void Init(
			GameStateUpdateService update, UnitService units, UnitFragment.Factory unitFragment, UnitsWindow.Factory unitsWindow,
			Canvas parent, string levelDesc, Action callback
		) {
			_update       = update;
			_units        = units;
			_unitFragment = unitFragment;
			_unitsWindow  = unitsWindow;
			_levelDesc    = levelDesc;
			_callback     = callback;
			
			_update.AddHandler<StartLevelCommand>(OnStartLevel);
			
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			StartButton.onClick.AddListener(OnStart);

			Header.text = levelDesc;
			FillUnits();
			UpdateInteractable();

			ShowAt(parent);
		}

		void OnDestroy() {
			_update.RemoveHandler<StartLevelCommand>(OnStartLevel);
		}
		
		Task OnStartLevel(StartLevelCommand _) {
			Animation.Hide(_callback);
			return Task.CompletedTask;
		}
		
		void OnStart() {
			_units.StartLevelWithSelectedUnits(_levelDesc, _selectedUnits);
		}
		
		ClickAction<UnitModel> OpenSelectWindow => new ClickAction<UnitModel>(
			"Select",
			u => {
				var selectableUnits = _units.GetSelectableUnitsForLevelExcept(_selectedUnits, SelectUnit);
				_selectWindow = _unitsWindow.Create(selectableUnits);
				_selectedUnitIndex = u.Index;
			}
		);
		
		ClickAction<UnitModel> SelectUnit => new ClickAction<UnitModel>(
			"Confirm",
			u => {
				var unit = (StateUnitModel)u;
				ReplaceUnit(_selectedUnitIndex, unit);
				UpdateInteractable();
				TryHideSelectWindow();
				_selectedUnitIndex = -1;
			}
		);

		void FillUnits() {
			var unitCount = 4;
			_selectedUnits = new UnitModel[unitCount];
			_fragments     = new UnitFragment[unitCount];
			foreach ( var unit in _units.GetUnitsForLevel(unitCount, OpenSelectWindow) ) {
				InsertUnit(unit);
			}
		}
		
		void InsertUnit(UnitModel unit) {
			_selectedUnits[unit.Index] = unit;
			_fragments[unit.Index]     = _unitFragment.Create(ItemsRoot, unit);
		}
		
		void ReplaceUnit(int index, StateUnitModel model) {
			var oldFragment = _fragments[index];
			oldFragment.gameObject.SetActive(false);
			var newModel = _units.CreateModel(model.State, index, OpenSelectWindow);
			InsertUnit(newModel);
			_fragments[index].transform.SetSiblingIndex(oldFragment.transform.GetSiblingIndex());
		}

		void TryHideSelectWindow() {
			if ( _selectWindow ) {
				_selectWindow.Hide();
				_selectWindow = null;
			}
		}
		
		void UpdateInteractable() {
			StartButton.interactable = _units.HasRealUnits(_selectedUnits);
		}
	}
}
