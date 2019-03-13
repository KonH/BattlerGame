using GameLogics.Models;
using Server.Utils;

namespace Server.Repositories.States {
	public interface IGameStateRepository {
		bool TryAdd(User user, Versioned<GameState> state);
		Versioned<GameState> Find(User user);
		Versioned<GameState> FindOrCreate(User user);
		void Save(User user, Versioned<GameState> state);
	}
}