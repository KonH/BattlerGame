using GameLogics.Core;

namespace GameLogics.Managers {
	public interface IGameStateManager {
		GameState State { get; }
		void Load();
		void Save();
	}
}