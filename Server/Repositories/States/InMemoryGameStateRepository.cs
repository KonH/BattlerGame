using System.Collections.Concurrent;
using GameLogics.Models;
using Server.Utils;

namespace Server.Repositories.States {
	public class InMemoryGameStateRepository : IGameStateRepository {
		ConcurrentDictionary<User, Versioned<GameState>> _states = new ConcurrentDictionary<User, Versioned<GameState>>();

		public bool TryAdd(User user, Versioned<GameState> state) {
			return _states.TryAdd(user, state);
		}
		
		public Versioned<GameState> Find(User user) {
			if ( _states.TryGetValue(user, out var state) ) {
				return state;
			}
			return null;
		}
		
		public Versioned<GameState> FindOrCreate(User user) {
			var state = Find(user);
			if ( state == null ) {
				state = new Versioned<GameState>(new GameState());
				Save(user, state);
			}
			return state;
		}

		public void Save(User user, Versioned<GameState> state) {
			_states[user] = state;
		}
	}
}