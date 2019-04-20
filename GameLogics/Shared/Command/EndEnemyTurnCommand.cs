using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;

namespace GameLogics.Shared.Command {
	public sealed class EndEnemyTurnCommand : IInternalCommand {
		public bool IsValid(GameState state, ConfigRoot config) {
			if ( state.Level == null ) {
				return false;
			}
			return !state.Level.PlayerTurn;
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer _) {
			state.Level.PlayerTurn = true;
			state.Level.MovedUnits.Clear();
		}

		public override string ToString() {
			return $"{nameof(EndEnemyTurnCommand)}";
		}
	}
}