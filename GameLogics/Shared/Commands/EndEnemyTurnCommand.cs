using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands {
	public class EndEnemyTurnCommand : IInternalCommand {
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return !state.Level.PlayerTurn;
		}

		public void Execute(GameState state, Config config, ICommandBuffer _) {
			state.Level.PlayerTurn = true;
			state.Level.MovedUnits.Clear();
		}

		public override string ToString() {
			return $"{nameof(EndEnemyTurnCommand)}";
		}
	}
}