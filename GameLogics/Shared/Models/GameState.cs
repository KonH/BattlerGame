using System;
using System.Collections.Generic;

namespace GameLogics.Shared.Models {
	public sealed class GameState {
		public string Version { get; set; } = string.Empty;
		
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();

		public GameState UpdateVersion() {
			Version = Guid.NewGuid().ToString();
			return this;
		}
	}
}