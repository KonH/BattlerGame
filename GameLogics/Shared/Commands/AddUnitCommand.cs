using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AddUnitCommand : IInternalCommand {
		public readonly ulong  Id;
		public readonly string Descriptor;

		public AddUnitCommand(ulong id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( state.Units.ContainsKey(Id) ) {
				return false;
			}
			return config.Units.ContainsKey(Descriptor);
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			var health = config.Units[Descriptor].MaxHealth;
			state.AddUnit(new UnitState(Descriptor, health).WithId(Id));
		}
		
		public override string ToString() {
			return $"{nameof(AddUnitCommand)} ({Id}, '{Descriptor}')";
		}
	}
}