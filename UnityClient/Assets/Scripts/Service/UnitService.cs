using System.Collections.Generic;
using System.Linq;
using GameLogics.Client.Service;
using GameLogics.Shared.Command;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;
using UnityClient.Model;

namespace UnityClient.Service {
	public sealed class UnitService {
		readonly ClientStateService  _stateService;
		readonly ClientCommandRunner _runner;
		
		GameState  State  => _stateService.State;
		ConfigRoot Config => _stateService.Config;

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

		public int GetMaxExperience(int level) {
			var levels = _stateService.Config.UnitLevels;
			return level < levels.Length ? levels[level] : 0;
		}

		public int GetMaxHealth(ulong unitId) {
			var state = GetUnitState(unitId);
			return GetUnitConfig(state.Descriptor).MaxHealth[state.Level];
		}

		public int GetBaseDamage(ulong unitId) {
			return DamageLogic.GetBaseDamage(State, Config, unitId);
		}

		public int GetWeaponDamage(ulong unitId) {
			return DamageLogic.GetWeaponDamage(State, Config, unitId);
		}

		public int GetAbsorb(ulong unitId) {
			return DamageLogic.GetAbsorb(State, Config, unitId);
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

		UnitState GetUnitState(ulong id) => State.Units[id];
		UnitConfig GetUnitConfig(string desc) => Config.Units[desc]; 
	}
}