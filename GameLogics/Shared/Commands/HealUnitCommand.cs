using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class HealUnitCommand : IInternalCommand {
		public readonly ulong UnitId;

		public HealUnitCommand(ulong unitId) {
			UnitId = unitId;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( !state.Units.TryGetValue(UnitId, out var unit) ) {
				return false;
			}
			return config.Units.ContainsKey(unit.Descriptor);
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			var unit = state.Units[UnitId];
			var maxHealth = config.Units[unit.Descriptor].MaxHealth;
			unit.Health = maxHealth;
		}

		public override string ToString() {
			return $"{nameof(HealUnitCommand)} ({UnitId})";
		}
	}
}