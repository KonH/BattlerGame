using GameLogics.Shared.Models;

namespace GameLogics.Shared.Commands {
	public class RemoveItemCommand : ICommand {
		public readonly string Id;

		public RemoveItemCommand(string id) {
			Id = id;
		}

		public bool IsValid(GameState state) => !string.IsNullOrEmpty(Id) && state.Items.ContainsKey(Id);
		
		public void Execute(GameState state) {
			state.Items.Remove(Id);
		}
		
		public override string ToString() {
			return string.Format("{0} ('{1}')", nameof(RemoveItemCommand), Id);
		}
	}
}