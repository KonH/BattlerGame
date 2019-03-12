using GameLogics.Core;

namespace GameLogics.Repositories.State {
	public sealed class InMemoryGameStateRepository : IGameStateRepository {
		public GameState State { get; private set; }
		
		public InMemoryGameStateRepository(GameState state) {
			State = state;
		}
	}
}