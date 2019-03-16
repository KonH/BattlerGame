using GameLogics.Shared.Models;

namespace GameLogics.Shared.Commands {
	public class AddItemCommand : ICommand {
		public readonly string Id;
		public readonly string Descriptor;

		public AddItemCommand(string id, string descriptor) {
			Id         = id;
			Descriptor = descriptor;
		}

		public bool IsValid(GameState state) {
			if ( string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(Descriptor) ) {
				return false;
			}
			return !state.Items.ContainsKey(Id);
		}

		public void Execute(GameState state) {
			state.AddItem(new ItemState(Descriptor).WithId(Id));
		}

		public override string ToString() {
			return string.Format("{0} ('{1}', '{2}')", nameof(AddItemCommand), Id, Descriptor);
		}
	}
}