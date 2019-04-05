using System.Collections.Generic;

namespace GameLogics.Shared.Models.State {
	public class UnitState {
		public ulong  Id         { get; set; }
		public string Descriptor { get; set; }
		public int    Health     { get; set; }
		
		public List<ItemState> Items { get; set; } = new List<ItemState>(); 

		public UnitState(string descriptor, int health) {
			Descriptor = descriptor;
			Health     = health;
		}

		public UnitState WithId(ulong id) {
			Id = id;
			return this;
		}
	}
}