using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class AddItemCommand : IInternalCommand {
		public readonly ulong  Id;
		public readonly string Descriptor;

		public AddItemCommand(ulong id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		public bool IsValid(GameState state, ConfigRoot config) {
			if ( string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( state.Items.ContainsKey(Id) ) {
				return false;
			}
			return config.Items.ContainsKey(Descriptor);
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			state.AddItem(new ItemState(Descriptor).WithId(Id));
		}

		public override string ToString() {
			return $"{nameof(AddItemCommand)} ({Id}, '{Descriptor}')";
		}
	}
}