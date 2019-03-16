using GameLogics.Shared.Models;

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

		public bool IsValid(GameState state) {
			if ( string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( Health < 0 ) {
				return false;
			}
			return !state.Units.ContainsKey(Id);
		}

		public void Execute(GameState state) {
			state.AddUnit(new UnitState(Descriptor, Health).WithId(Id));
		}
		
		public override string ToString() {
			return string.Format("{0} ('{1}', '{2}', {3})", nameof(AddUnitCommand), Id, Descriptor, Health);
		}
	}
}