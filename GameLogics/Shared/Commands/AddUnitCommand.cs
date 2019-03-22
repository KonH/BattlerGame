using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AddUnitCommand : IInternalCommand {
		public readonly ulong  Id;
		public readonly string Descriptor;
		public readonly int    Health;

		public AddUnitCommand(ulong id, string descriptor, int health) {
			Id         = id;
			Descriptor = descriptor;
			Health     = health;
		}

		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( Health < 0 ) {
				return false;
			}
			if ( state.Units.ContainsKey(Id) ) {
				return false;
			}
			return config.Units.ContainsKey(Descriptor);
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.AddUnit(new UnitState(Descriptor, Health).WithId(Id));
		}
		
		public override string ToString() {
			return $"{nameof(AddUnitCommand)} ({Id}, '{Descriptor}', {Health})";
		}
	}
}