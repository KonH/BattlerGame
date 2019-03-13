using GameLogics.Models;

namespace GameLogics.Repositories.State {
	public interface IGameStateRepository {
		string    Version { get; set; }
		GameState State   { get; set; }
	}
}