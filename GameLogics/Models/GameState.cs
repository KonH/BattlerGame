using System.Collections.Generic;

namespace GameLogics.Models {
	public sealed class GameState {
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();
	}
}