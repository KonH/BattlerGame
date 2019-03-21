using System;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Services {
	public class StateInitService {
		class InitCommand : BaseCommand {
			protected override bool IsValid(GameState state, Config config) => true;

			protected override void Execute(GameState state, Config config, ICommandBuffer buffer) {
				buffer.AddCommand(new AddResourceCommand(Resource.Coins, 50));
				buffer.AddCommand(new AddUnitCommand(state.NewEntityId(), "player_unit", 5));
			}
		}
		
		public GameState Init(GameState state, Config config) {
			Execute(new InitCommand(), state, config);
			return state;
		}

		void Execute(ICompositeCommand command, GameState state, Config config) {
			foreach ( var cmd in command.AsEnumerable() ) {
				if ( !cmd.IsCommandValid(state, config) ) {
					throw new InvalidOperationException($"Can't init state with {command}");
				}
				cmd.ExecuteCommand(state, config);
			}
		}
	}
}