using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class AddItemCommand : ICommand {
		public readonly string Id;
		public readonly string Descriptor;

		public AddItemCommand(string id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		public bool IsValid(GameState state, Config config) {
			if ( string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			if ( state.Items.ContainsKey(Id) ) {
				return false;
			}
			return config.Items.ContainsKey(Descriptor);
		}

		public void Execute(GameState state, Config config) {
			state.AddItem(new ItemState(Descriptor).WithId(Id));
		}

		public override string ToString() {
			return $"{nameof(AddItemCommand)} ('{Id}', '{Descriptor}')";
		}
	}
}