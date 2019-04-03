using System;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class StartLevelWindow : BaseWindow {		
		public class Factory : PlaceholderFactory<string, Action, StartLevelWindow> {}
		
		public Button    CloseButton;
		public Button    StartButton;
		public Transform ItemsRoot;

		string              _levelDesc;
		ClientStateService  _service;
		ClientCommandRunner _runner;

		UnitModel[] _units;
		
		UnitFragment[] _fragments;

		UnitFragment.Factory _unitFragment;

		UnitsWindow.Factory _unitsWindow;

		UnitsWindow _selectWindow;
		
		Action _onStart;
		
		[Inject]
		public void Init(
			ClientStateService service, ClientCommandRunner runner, UnitFragment.Factory unitFragment, UnitsWindow.Factory unitsWindow,
			Canvas parent, string levelDesc, Action callback
		) {
			_service      = service;
			_runner       = runner;
			_unitFragment = unitFragment;
			_unitsWindow  = unitsWindow;
			_levelDesc    = levelDesc;
			_onStart      = callback;
			
			_runner.Updater.AddHandler<StartLevelCommand>(OnStartLevel);
			
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			StartButton.onClick.AddListener(OnStart);

			var allUnits = _service.State.Units.Values.OrderBy(u => u.Id).ToList();
			var unitCount = 4;
			_units = new UnitModel[unitCount];
			_fragments = new UnitFragment[unitCount];
			for ( var i = 0; i < unitCount; i++ ) {
				if ( allUnits.Count <= i ) {
					InsertPlaceholderUnit(i);
				} else {
					var state = allUnits[i];
					InsertStateUnit(state, i);
				}
			}
			
			UpdateInteractable();

			ShowAt(parent);
		}

		void OnDestroy() {
			_runner?.Updater.RemoveHandler<StartLevelCommand>(OnStartLevel);
		}

		void InsertStateUnit(UnitState state, int index) {
			InsertUnit(new StateUnitModel(state, index, new ClickAction<UnitModel>("Select", OnUnitSelected)), index);
		}

		void InsertPlaceholderUnit(int index) {
			InsertUnit(new PlaceholderUnitModel(index, new ClickAction<UnitModel>("Select", OnUnitSelected)), index);
		}
		
		void InsertUnit(UnitModel unit, int index) {
			_units[index]     = unit;
			_fragments[index] = _unitFragment.Create(ItemsRoot, unit);
		}

		void OnUnitSelected(UnitModel unit) {
			var selectableUnits = _runner.Updater.State.Units.Values
				.Where(u => _units.Where(m => m is StateUnitModel).Cast<StateUnitModel>().All(m => m.State.Id != u.Id))
				.Select(u => new StateUnitModel(u, unit.Index, new ClickAction<UnitModel>("Apply", OnUnitApplied)) as UnitModel)
				.ToList();

			_selectWindow = _unitsWindow.Create(selectableUnits);
		}

		void OnUnitApplied(UnitModel rawUnit) {
			var unit = (StateUnitModel)rawUnit;
			ReplaceUnit(unit.Index, unit.State);
			UpdateInteractable();

			if ( _selectWindow ) {
				_selectWindow.Hide();
				_selectWindow = null;
			}
		}

		void ReplaceUnit(int index, UnitState state) {
			var oldFragment = _fragments[index];
			oldFragment.gameObject.SetActive(false);
			InsertStateUnit(state, index);
			_fragments[index].transform.SetSiblingIndex(oldFragment.transform.GetSiblingIndex());
		}
		
		void UpdateInteractable() {
			StartButton.interactable = _units.Any(u => u is StateUnitModel);
		}

		void OnStart() {
			var unitIds = _units.Where(u => u is StateUnitModel).Cast<StateUnitModel>().Select(u => u.State.Id).ToList();
			_runner.TryAddCommand(new StartLevelCommand(_levelDesc, unitIds));
		}
		
		Task OnStartLevel(StartLevelCommand _) {
			Animation.Hide(_onStart);
			return Task.CompletedTask;
		}
	}
}
