using System;
using GameLogics.Server.Model;
using GameLogics.Shared.Model.State;

namespace GameLogics.Server.Repository.State {
	public interface IGameStateRepository {
		GameState Find(UserState user);
		GameState FindOrCreate(UserState user, Action<GameState> init);
		GameState Save(UserState user, GameState state);
	}
}