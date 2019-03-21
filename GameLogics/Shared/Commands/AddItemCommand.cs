using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AddItemCommand : InternalCommand {
		public readonly ulong  Id;
		public readonly string Descriptor;

		public AddItemCommand(ulong id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		protected override bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( state.Items.ContainsKey(Id) ) {
				return false;
			}
			return config.Items.ContainsKey(Descriptor);
		}

		protected override void Execute(GameState state, Config config, ICommandBuffer _) {
			state.AddItem(new ItemState(Descriptor).WithId(Id));
		}

		public override string ToString() {
			return $"{nameof(AddItemCommand)} ({Id}, '{Descriptor}')";
		}
	}
}