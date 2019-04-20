using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class AddUnitCommand : IInternalCommand {
		public readonly ulong  Id;
		public readonly string Descriptor;

		public AddUnitCommand(ulong id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		public bool IsValid(GameState state, ConfigRoot config) {
			if ( string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( state.Units.ContainsKey(Id) ) {
				return false;
			}
			return config.Units.ContainsKey(Descriptor);
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var health = config.Units[Descriptor].MaxHealth[0];
			state.AddUnit(new UnitState(Descriptor, health).WithId(Id));
		}
		
		public override string ToString() {
			return $"{nameof(AddUnitCommand)} ({Id}, '{Descriptor}')";
		}
	}
}