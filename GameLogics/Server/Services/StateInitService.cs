using System;
using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Server.Services {
	public class StateInitService {
		readonly List<Func<GameState, ICommand>> _initCommands = new List<Func<GameState, ICommand>> {
			_ => new AddResourceCommand(Resource.Coins, 50),
			s => new AddUnitCommand(s.NewEntityId(), "player_unit", 1)
		};
		
		public GameState Init(GameState state, Config config) {
			foreach ( var cmd in _initCommands ) {
				Execute(cmd(state), state, config);
			}
			return state;
		}

		void Execute(ICommand command, GameState state, Config config) {
			if ( !command.TryExecute(state, config) ) {
				throw new InvalidOperationException($"Can't init state with {command}");
			}
		}
	}
}