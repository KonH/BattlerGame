using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class EndEnemyTurnCommand : InternalCommand {
		protected override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return !state.Level.PlayerTurn;
		}

		protected override void Execute(GameState state, Config config, ICommandBuffer _) {
			state.Level.PlayerTurn = true;
			state.Level.MovedUnits.Clear();
		}

		public override string ToString() {
			return $"{nameof(EndEnemyTurnCommand)}";
		}
	}
}