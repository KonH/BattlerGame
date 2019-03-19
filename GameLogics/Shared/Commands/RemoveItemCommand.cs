using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class RemoveItemCommand : BaseCommand {
		public readonly string Id;

		public RemoveItemCommand(string id) {
			Id = id;
		}

		public override bool IsValid(GameState state, Config config) => !string.IsNullOrEmpty(Id) && state.Items.ContainsKey(Id);
		
		protected override void ExecuteSingle(GameState state, Config config) {
			state.Items.Remove(Id);
		}
		
		public override string ToString() {
			return $"{nameof(RemoveItemCommand)} ('{Id}')";
		}
	}
}