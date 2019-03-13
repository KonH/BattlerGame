using GameLogics.Models;
using GameLogics.Server.Utils;

namespace GameLogics.Server.Repositories.States {
	public interface IGameStatesRepository {
		bool TryAdd(User user, Versioned<GameState> state);
		Versioned<GameState> Find(User user);
		Versioned<GameState> FindOrCreate(User user);
		void Save(User user, Versioned<GameState> state);
	}
}