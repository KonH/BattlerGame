using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class HealUnitCommand : IInternalCommand {
		public readonly ulong UnitId;

		public HealUnitCommand(ulong unitId) {
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, ConfigRoot config) {
			if ( !state.Units.TryGetValue(UnitId, out var unit) ) {
				return false;
			}
			return config.Units.ContainsKey(unit.Descriptor);
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var unit = state.Units[UnitId];
			var maxHealth = config.Units[unit.Descriptor].MaxHealth[unit.Level];
			unit.Health = maxHealth;
		}

		public override string ToString() {
			return $"{nameof(HealUnitCommand)} ({UnitId})";
		}
	}
}