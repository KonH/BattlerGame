using GameLogics.Core;

namespace GameLogics.Repositories.State {
	public interface IGameStateRepository {
		GameState State { get; }
	}
}