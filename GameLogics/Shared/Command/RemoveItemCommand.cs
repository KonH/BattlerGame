using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class RemoveItemCommand : ICommand {
		public readonly ulong Id;

		public RemoveItemCommand(ulong id) {
			Id = id;
		}

		public bool IsValid(GameState state, ConfigRoot config) => state.Items.ContainsKey(Id);
		
		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			state.Items.Remove(Id);
		}
		
		public override string ToString() {
			return $"{nameof(RemoveItemCommand)} ({Id})";
		}
	}
}