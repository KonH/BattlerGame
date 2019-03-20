using System.Collections.Generic;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Models {
	public sealed class GameState {
		public string Version { get; set; } = string.Empty;
		
		public ulong                        EntityId  { get; set; }
		public Dictionary<Resource, int>    Resources { get; } = new Dictionary<Resource, int>();
		public Dictionary<ulong, ItemState> Items     { get; } = new Dictionary<ulong, ItemState>();
		public Dictionary<ulong, UnitState> Units     { get; } = new Dictionary<ulong, UnitState>();
		
		public LevelState Level { get; set; } = null;

		public GameState UpdateVersion() {
			Version = UniqueId.New();
			return this;
		}

		public ulong NewEntityId() {
			EntityId++;
			return EntityId;
		}

		public GameState AddItem(ItemState item) {
			Items.Add(item.Id, item);
			return this;
		}

		public GameState AddUnit(UnitState unit) {
			Units.Add(unit.Id, unit);
			return this;
		}
	}
}