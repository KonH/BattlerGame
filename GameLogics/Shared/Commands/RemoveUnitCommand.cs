using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class RemoveUnitCommand : BaseCommand {
		public readonly ulong Id;

		public RemoveUnitCommand(ulong id) {
			Id = id;
		}

		protected override bool IsValid(GameState state, Config config) => state.Units.ContainsKey(Id);
		
		protected override void Execute(GameState state, Config config, ICommandBuffer _) {
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