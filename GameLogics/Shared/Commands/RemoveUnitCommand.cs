using GameLogics.Shared.Models;

namespace GameLogics.Shared.Commands {
	public class RemoveUnitCommand : ICommand {
		public readonly string Id;

		public RemoveUnitCommand(string id) {
			Id = id;
		}

		public bool IsValid(GameState state) => !string.IsNullOrEmpty(Id) && state.Units.ContainsKey(Id);
		
		public void Execute(GameState state) {
			state.Units.Remove(Id);
		}
		
		public override string ToString() {
			return string.Format("{0} ('{1}')", nameof(RemoveUnitCommand), Id);
		}
	}
}