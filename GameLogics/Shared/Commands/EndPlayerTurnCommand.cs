using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;

namespace GameLogics.Shared.Commands {
	public class EndPlayerTurnCommand : BaseCommand {
		public override bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return state.Level.PlayerTurn;
		}

		public override List<ICommand> Execute(GameState state, Config config) {
			state.Level.PlayerTurn = false;
			state.Level.MovedUnits.Clear();

			return LevelAiService.CreateCommands(state, config);
		}

		public override string ToString() {
			return $"{nameof(EndPlayerTurnCommand)}";
		}
	}
}