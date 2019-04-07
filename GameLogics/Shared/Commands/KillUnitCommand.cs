using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public sealed class KillUnitCommand : IInternalCommand {
		public readonly ulong UnitId;
		
		public KillUnitCommand(ulong unitId) {
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			var level = state.Level;
			if ( HasHealthOrInvalid(level.FindUnitById(UnitId)) ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			var level = state.Level;
			if ( TryKill(level.PlayerUnits, out var player) ) {
				if ( config.IsFeatureEnabled(Features.AutoHeal) ) {
					buffer.Add(new HealUnitCommand(player.Id));
				}
				state.Units.Add(UnitId, player);
				if ( level.PlayerUnits.Count == 0 ) {
					buffer.Add(new FinishLevelCommand(false));
				}
			}
			TryKill(level.EnemyUnits, out _);
			if ( level.EnemyUnits.Count == 0 ) {
				buffer.Add(new FinishLevelCommand(true));
			}
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
			return $"{nameof(KillUnitCommand)} ({UnitId})";
		}
	}
}