using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;

namespace GameLogics.Shared.Commands {
	public class EndPlayerTurnCommand : ICommand {
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return state.Level.PlayerTurn;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			state.Level.PlayerTurn = false;
			state.Level.MovedUnits.Clear();

			LevelAiService.AddCommands(state, config, buffer);
		}

		public override string ToString() {
			return $"{nameof(EndPlayerTurnCommand)}";
		}
	}
}