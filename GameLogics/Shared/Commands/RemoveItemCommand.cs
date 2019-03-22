using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class RemoveItemCommand : ICommand {
		public readonly ulong Id;

		public RemoveItemCommand(ulong id) {
			Id = id;
		}

		public bool IsValid(GameState state, Config config) => state.Items.ContainsKey(Id);
		
		public void Execute(GameState state, Config config, ICommandBuffer _) {
			state.Items.Remove(Id);
		}
		
		public override string ToString() {
			return $"{nameof(RemoveItemCommand)} ({Id})";
		}
	}
}