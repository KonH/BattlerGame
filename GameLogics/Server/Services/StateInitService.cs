using System;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Services {
	public class StateInitService {
		class InitCommand : ICommand {
			public bool IsValid(GameState state, Config config) => true;

			public void Execute(GameState state, Config config, ICommandBuffer buffer) {
				buffer.Add(new AddResourceCommand(Resource.Coins, 50));
				buffer.Add(new AddUnitCommand(state.NewEntityId(), "player_unit"));
			}
		}
		
		public GameState Init(GameState state, Config config) {
			var runner = new CommandRunner(new InitCommand(), state, config);
			foreach ( var item in runner ) {
				if ( !item.IsValid() ) {
					throw new InvalidOperationException($"Can't init state with {item.Command}");
				}
				item.Execute();
			}
			return state;
		}
	}
}