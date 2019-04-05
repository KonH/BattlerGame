using System;
using System.Collections.Concurrent;
using GameLogics.Server.Models;
using GameLogics.Shared.Models.State;

namespace GameLogics.Server.Repositories.States {
	public class InMemoryGameStatesRepository : IGameStatesRepository {
		ConcurrentDictionary<User, GameState> _states = new ConcurrentDictionary<User, GameState>();
		
		public GameState Find(User user) {
			if ( _states.TryGetValue(user, out var state) ) {
				return state;
			}
			return null;
		}
		
		public GameState FindOrCreate(User user, Action<GameState> init) {
			var state = Find(user);
			if ( state == null ) {
				state = new GameState();
				init(state);
			}
			return state;
		}

		public GameState Save(User user, GameState state) {
			_states[user] = state;
			return state;
		}
	}
}