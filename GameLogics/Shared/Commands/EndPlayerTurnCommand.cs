using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Logics;

namespace GameLogics.Shared.Commands {
	public sealed class EndPlayerTurnCommand : ICommand {
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return state.Level.PlayerTurn;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.Level.PlayerTurn = false;
			state.Level.MovedUnits.Clear();

			LevelAiLogics.AddCommands(state, config, buffer);
		}

		public override string ToString() {
			return $"{nameof(EndPlayerTurnCommand)}";
		}
	}
}