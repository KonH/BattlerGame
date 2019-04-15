using System;
using System.Collections.Concurrent;
using GameLogics.Server.Model;
using GameLogics.Shared.Model.State;

namespace GameLogics.Server.Repository.State {
	public sealed class InMemoryGameStateRepository : IGameStateRepository {
		ConcurrentDictionary<UserState, GameState> _states = new ConcurrentDictionary<UserState, GameState>();
		
		public GameState Find(UserState user) {
			if ( _states.TryGetValue(user, out var state) ) {
				return state;
			}
			return null;
		}
		
		public GameState FindOrCreate(UserState user, Action<GameState> init) {
			var state = Find(user);
			if ( state == null ) {
				state = new GameState();
				init(state);
			}
			return state;
		}

		public GameState Save(UserState user, GameState state) {
			_states[user] = state;
			return state;
		}
	}
}