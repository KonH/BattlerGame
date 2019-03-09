using GameLogics.Core;

namespace GameLogics.Managers {
	public sealed class InMemoryGameStateManager : IGameStateManager {
		public GameState State { get; private set; }
		
		public InMemoryGameStateManager(GameState state) {
			State = state;
		}

		public void Load() {}
		public void Save() {}
	}
}