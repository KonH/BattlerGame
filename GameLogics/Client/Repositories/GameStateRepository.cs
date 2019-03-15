using GameLogics.Shared.Models;

namespace GameLogics.Client.Repositories {
	public sealed class GameStateRepository {
		public string    Version { get; set; }
		public GameState State   { get; set; }
	}
}