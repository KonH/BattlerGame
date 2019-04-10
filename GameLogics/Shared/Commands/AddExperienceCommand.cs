using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;

namespace GameLogics.Shared.Commands {
	public sealed class AddExperienceCommand : IInternalCommand {
		public readonly ulong UnitId;
		public readonly int   Amount;

		public AddExperienceCommand(ulong unitId, int amount) {
			UnitId = unitId;
			Amount = amount;
		}

		public bool IsValid(GameState state, Config config) {
			if ( Amount <= 0 ) {
				return false;
			}
			if ( !state.Units.TryGetValue(UnitId, out var unit) ) {
				return false;
			}
			if ( unit.Level == config.UnitLevels.Length ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			var unit = state.Units[UnitId];
			unit.Experience += Amount;
			if ( unit.Experience >= config.UnitLevels[unit.Level] ) {
				buffer.Add(new LevelUpCommand(UnitId));
			}
		}

		public override string ToString() {
			return $"{nameof(AddExperienceCommand)} ({UnitId}, {Amount})";
		}
	}
}
