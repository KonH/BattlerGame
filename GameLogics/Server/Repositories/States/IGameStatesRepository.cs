using System;
using GameLogics.Server.Models;
using GameLogics.Shared.Models.State;

namespace GameLogics.Server.Repositories.States {
	public interface IGameStatesRepository {
		GameState Find(User user);
		GameState FindOrCreate(User user, Action<GameState> init);
		GameState Save(User user, GameState state);
	}
}