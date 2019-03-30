using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;

namespace UnityClient.ViewModels.Windows {
	public class StartLevelWindow : BaseWindow {
		public Button    CloseButton;
		public Button    StartButton;
		public Transform ItemsRoot;

		public BaseAnimation Animation;

		string              _levelDesc;
		ClientStateService  _service;
		ClientCommandRunner _runner;
		UnitFragment        _unitTemplate;
		
		Action                                             _onStart;
		Action<List<UnitModel>, string, Action<UnitModel>> _onSelectUnit;

		List<UnitModel> _units = new List<UnitModel>();
		
		List<UnitFragment> _fragments = new List<UnitFragment>(); 
		
		void Awake() {
			Animation.Show();
		}

		public void Show(
			string levelDesc,
			ClientStateService service, ClientCommandRunner runner,
			UnitFragment unitTemplate, Action onStart, Action<List<UnitModel>, string, Action<UnitModel>> onSelectUnit
		) {
			_levelDesc    = levelDesc;
			_service      = service;
			_runner       = runner;
			_unitTemplate = unitTemplate;
			_onStart      = onStart;
			_onSelectUnit = onSelectUnit;
			
			_runner.Updater.AddHandler<StartLevelCommand>(OnStartLevel);
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			StartButton.onClick.AddListener(OnStart);
			_unitTemplate.transform.SetParent(ItemsRoot, false);
			_unitTemplate.gameObject.SetActive(false);

			var allUnits = _service.State.Units.Values.OrderBy(u => u.Id).ToList();
			for ( var i = 0; i < 4; i++ ) {
				if ( allUnits.Count <= i ) {
					AddUnit(new UnitModel(i));
				} else {
					var state = allUnits[i];
					var config = _service.Config.Units[state.Descriptor];
					AddUnit(new UnitModel(true, state, config, i));
				}
			}
			
			UpdateInteractable();
		}

		void OnDestroy() {
			_runner?.Updater.RemoveHandler<StartLevelCommand>(OnStartLevel);
		}

		void AddUnit(UnitModel unit) {
			_units.Add(unit);
			var instance = CreateFragment(unit);
			_fragments.Add(instance);
		}

		UnitFragment CreateFragment(UnitModel unit) {
			var instance = Instantiate(_unitTemplate, ItemsRoot, false);
			instance.gameObject.SetActive(true);
			instance.Init(unit, "Select", OnUnitSelected);
			return instance;
		}

		void OnUnitSelected(UnitModel unit) {
			var selectableUnits = _runner.Updater.State.Units.Values
				.Where(u => _units.Find(m => !m.IsFake && (m.State.Id == u.Id)) == null)
				.Select(u => new UnitModel(false, u, _service.Config.Units[u.Descriptor], unit.Index))
				.ToList();
			
			_onSelectUnit(selectableUnits, "Select", OnUnitApplied);
		}

		void OnUnitApplied(UnitModel unit) {
			_units[unit.Index] = unit;
			var oldFragment = _fragments[unit.Index];
			oldFragment.gameObject.SetActive(false);
			var newFragment = CreateFragment(unit);
			newFragment.transform.SetSiblingIndex(oldFragment.transform.GetSiblingIndex());
			_fragments[unit.Index] = newFragment;
			UpdateInteractable();
		}
		
		void UpdateInteractable() {
			StartButton.interactable = _units.Any(u => !u.IsFake);
		}

		void OnStart() {
			var unitIds = _units.Where(u => !u.IsFake).Select(u => u.State.Id).ToList();
			_runner.TryAddCommand(new StartLevelCommand(_levelDesc, unitIds));
		}
		
		Task OnStartLevel(StartLevelCommand _) {
			Animation.Hide(_onStart);
			return Task.CompletedTask;
		}
	}
}
