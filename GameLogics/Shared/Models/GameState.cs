using System.Collections.Generic;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Models {
	public sealed class GameState {
		public string Version { get; set; } = string.Empty;
		
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();
		public Dictionary<string, ItemState> Items { get; } = new Dictionary<string, ItemState>();

		public GameState UpdateVersion() {
			Version = UniqueId.New();
			return this;
		}

		public GameState AddItem(ItemState item) {
			Items.Add(item.Id, item);
			return this;
		}
	}
}