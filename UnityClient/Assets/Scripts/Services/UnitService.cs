using System.Collections.Generic;
using System.Linq;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using UnityClient.Models;

namespace UnityClient.Services {
	public sealed class UnitService {
		readonly ClientStateService  _stateService;
		readonly ClientCommandRunner _runner;
		
		GameState State => _stateService.State;

		public UnitService(ClientStateService stateService, ClientCommandRunner runner) {
			_stateService = stateService;
			_runner       = runner;
		}

		public UnitModel CreateModel(UnitState state, int index, ClickAction<UnitModel> onClick) {
			return new StateUnitModel(state, index, onClick);
		}

		public UnitModel CreatePlaceholder(int index, ClickAction<UnitModel> onClick) {
			return new PlaceholderUnitModel(index, onClick);
		}
		
		UnitModel CreateModel(int index, List<UnitState> states, ClickAction<UnitModel> onUnit, ClickAction<UnitModel> onPlaceholder) {
			if ( states.Count > index ) {
				return CreateModel(states[index], index, onUnit);
			}
			return CreatePlaceholder(index, onPlaceholder);
		}
		
		public List<UnitModel> GetUnitsForLevel(int count, ClickAction<UnitModel> onUnit, ClickAction<UnitModel> onPlaceholder) {
			var result = new List<UnitModel>();
			var units = GetAllUnitsForLevel().OrderBy(u => u.Id).ToList();
			for ( var i = 0; i < count; i++ ) {
				result.Add(CreateModel(i, units, onUnit, onPlaceholder));
			}
			return result;
		}

		public List<UnitModel> GetSelectableUnitsForLevelExcept(UnitModel[] exceptions, ClickAction<UnitModel> onClick) {
			var result = new List<UnitModel>();
			var realExceptions = FilterRealUnits(exceptions);
			var units = GetAllUnitsForLevel();
			foreach ( var unit in units ) {
				if ( IsException(unit, realExceptions) ) {
					continue;
				}
				result.Add(CreateModel(unit, -1, onClick));
			}
			return result;
		}

		public List<StateUnitModel> FilterRealUnits(UnitModel[] units) {
			var result = new List<StateUnitModel>();
			foreach ( var unit in units ) {
				if ( (unit is StateUnitModel stateUnit) ) {
					result.Add(stateUnit);
				}
			}
			return result;
		}

		public bool HasRealUnits(UnitModel[] units) => FilterRealUnits(units).Count > 0;

		public void StartLevelWithSelectedUnits(string levelDesc, UnitModel[] units) {
			var unitIds = GetUnitIds(units);
			_runner.TryAddCommand(new StartLevelCommand(levelDesc, unitIds));
		}
		
		List<UnitState> GetAllUnitsForLevel() => State.Units.Values.ToList();
		
		bool IsException(UnitState unit, List<StateUnitModel> exceptions) {
			foreach ( var exception in exceptions ) {
				if ( unit.Id == exception.State.Id ) {
					return true;
				}
			}
			return false;
		}
		
		List<ulong> GetUnitIds(UnitModel[] units) {
			var result    = new List<ulong>();
			var realUnits = FilterRealUnits(units);
			foreach ( var unit in realUnits ) {
				result.Add(unit.State.Id);
			}
			return result;
		}
	}
}