using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class RemoveUnitCommand : BaseCommand {
		public readonly string Id;

		public RemoveUnitCommand(string id) {
			Id = id;
		}

		public override bool IsValid(GameState state, Config config) => !string.IsNullOrEmpty(Id) && state.Units.ContainsKey(Id);
		
		protected override void ExecuteSingle(GameState state, Config config) {
			var unit = state.Units[Id];
			foreach ( var item in unit.Items ) {
				state.Items.Add(item.Id, item);
			}
			state.Units.Remove(Id);
		}
		
		public override string ToString() {
			return $"{nameof(RemoveUnitCommand)} ('{Id}')";
		}
	}
}