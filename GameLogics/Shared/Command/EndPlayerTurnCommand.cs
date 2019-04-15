using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Logic;

namespace GameLogics.Shared.Command {
	public sealed class EndPlayerTurnCommand : ICommand {
		public bool IsValid(GameState state, ConfigRoot config) {
			if ( state.Level == null ) {
				return false;
			}
			return state.Level.PlayerTurn;
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			state.Level.PlayerTurn = false;
			state.Level.MovedUnits.Clear();

			LevelAiLogic.AddCommands(state, config, buffer);
		}

		public override string ToString() {
			return $"{nameof(EndPlayerTurnCommand)}";
		}
	}
}