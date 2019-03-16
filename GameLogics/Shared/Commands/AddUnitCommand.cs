using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AddUnitCommand : ICommand {
		public readonly string Id;
		public readonly string Descriptor;
		public readonly int    Health;

		public AddUnitCommand(string id, string descriptor, int health) {
			Id         = id;
			Descriptor = descriptor;
			Health     = health;
		}

		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Descriptor) ) {
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

		public void Execute(GameState state, Config config) {
			state.AddUnit(new UnitState(Descriptor, Health).WithId(Id));
		}
		
		public override string ToString() {
			return string.Format("{0} ('{1}', '{2}', {3})", nameof(AddUnitCommand), Id, Descriptor, Health);
		}
	}
}