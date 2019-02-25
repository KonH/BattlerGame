using System.Collections.Generic;

namespace GameLogics.Core {
	public sealed class GameState {
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();
	}
}