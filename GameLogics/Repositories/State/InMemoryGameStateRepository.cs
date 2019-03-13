using GameLogics.Models;

namespace GameLogics.Repositories.State {
	public sealed class InMemoryGameStateRepository : IGameStateRepository {
		public string    Version { get; set; }
		public GameState State   { get; set; }
	}
}