using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Command {
	public sealed class LevelUpCommand : IInternalCommand {
		public readonly ulong UnitId;

		public LevelUpCommand(ulong unitId) {
			UnitId = unitId;
		}

		public bool IsValid(GameState state, ConfigRoot config) {
			if ( !state.Units.TryGetValue(UnitId, out var unit) ) {
				return false;
			}
			if ( unit.Level >= config.UnitLevels.Length ) {
				return false;
			}
			if ( unit.Experience < config.UnitLevels[unit.Level] ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var unit = state.Units[UnitId];
			var oldLevel = unit.Level;
			unit.Level++;
			unit.Experience -= config.UnitLevels[oldLevel];
			var isLastLevel = (unit.Level == config.UnitLevels.Length);
			if ( isLastLevel ) {
				unit.Experience = 0;
			}
			
			buffer.Add(new HealUnitCommand(UnitId));

			if ( !isLastLevel && (unit.Experience >= config.UnitLevels[unit.Level]) ) {
				buffer.Add(new LevelUpCommand(UnitId));
			}
		}

		public override string ToString() {
			return $"{nameof(LevelUpCommand)} ({UnitId})";
		}
	}
}
