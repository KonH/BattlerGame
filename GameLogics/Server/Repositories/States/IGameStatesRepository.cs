using GameLogics.Server.Models;
using GameLogics.Shared.Models;

namespace GameLogics.Server.Repositories.States {
	public interface IGameStatesRepository {
		GameState Find(User user);
		GameState FindOrCreate(User user);
		GameState Save(User user, GameState state);
	}
}