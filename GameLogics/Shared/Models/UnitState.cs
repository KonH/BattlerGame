using System.Collections.Generic;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Models {
	public class UnitState {
		public string Id         { get; set; }
		public string Descriptor { get; set; }
		public int    Health     { get; set; }
		
		public List<ItemState> Items { get; set; } = new List<ItemState>(); 

		public UnitState(string descriptor, int health) {
			Descriptor = descriptor;
			Health     = health;
		}

		public UnitState WithId(string id) {
			Id = id;
			return this;
		}

		public UnitState WithNewId() => WithId(UniqueId.New());
	}
}