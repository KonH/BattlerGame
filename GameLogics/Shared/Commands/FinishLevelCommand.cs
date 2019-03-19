using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public class FinishLevelCommand : InternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		protected override void ExecuteSingle(GameState state, Config config) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
			}
			state.Level = null;

			// temp
			state.Resources[Resource.Coins] = state.Resources.GetOrDefault(Resource.Coins) + 100;
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}