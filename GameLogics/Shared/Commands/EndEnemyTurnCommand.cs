using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class EndEnemyTurnCommand : InternalCommand {
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return !state.Level.PlayerTurn;
		}

		protected override void ExecuteSingle(GameState state, Config config) {
			state.Level.PlayerTurn = true;
			state.Level.MovedUnits.Clear();
		}

		public override string ToString() {
			return $"{nameof(EndEnemyTurnCommand)}";
		}
	}
}