using System;
using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Server.Services {
	public class StateInitService {
		readonly List<ICommand> _initCommands = new List<ICommand> {
			new AddResourceCommand(Resource.Coins, 50),
			new AddUnitCommand(UniqueId.New(), "test_unit", 1)
		};
		
		public GameState Init(GameState state, Config config) {
			foreach ( var cmd in _initCommands ) {
				Execute(cmd, state, config);
			}
			return state;
		}

		void Execute(ICommand command, GameState state, Config config) {
			if ( !command.IsValid(state, config) ) {
				throw new InvalidOperationException($"Can't init state with {command}");
			}
			command.Execute(state, config);
		}
	}
}