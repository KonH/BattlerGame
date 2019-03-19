using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class KillUnitCommand : InternalCommand {
		public readonly ulong UnitId;
		
		public KillUnitCommand(ulong unitId) {
			UnitId = unitId;
		}
		
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			var level = state.Level;
			if ( HasHealthOrInvalid(level.FindUnitById(UnitId)) ) {
				return false;
			}
			return true;
		}

		public override List<ICommand> Execute(GameState state, Config config) {
			var level = state.Level;
			if ( TryKill(level.PlayerUnits, out var player) ) {
				state.Units.Add(UnitId, player);
				if ( level.PlayerUnits.Count == 0 ) {
					return WithSubCommand(new FinishLevelCommand(false));
				}
			}
			TryKill(level.EnemyUnits, out _);
			if ( level.EnemyUnits.Count == 0 ) {
				return WithSubCommand(new FinishLevelCommand(true));
			}
			return NoSubCommands;
		}
		
		UnitState FindUnit(List<UnitState> units) {
			return units.Find(u => u.Id == UnitId);
		}

		bool HasHealthOrInvalid(UnitState unit) {
			return (unit == null) || (unit.Health > 0);
		}

		bool TryKill(List<UnitState> units, out UnitState unit) {
			unit = FindUnit(units);
			if ( unit != null ) {
				units.Remove(unit);
				return true;
			}
			return false;
		}

		public override string ToString() {
			return $"{nameof(KillUnitCommand)} ('{UnitId}')";
		}
	}
}