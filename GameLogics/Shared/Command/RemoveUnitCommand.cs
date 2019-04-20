using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class RemoveUnitCommand : ICommand {
		public readonly ulong Id;

		public RemoveUnitCommand(ulong id) {
			Id = id;
		}

		public bool IsValid(GameState state, ConfigRoot config) => state.Units.ContainsKey(Id);
		
		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			var unit = state.Units[Id];
			foreach ( var item in unit.Items ) {
				state.Items.Add(item.Id, item);
			}
			state.Units.Remove(Id);
		}
		
		public override string ToString() {
			return $"{nameof(RemoveUnitCommand)} ({Id})";
		}
	}
}