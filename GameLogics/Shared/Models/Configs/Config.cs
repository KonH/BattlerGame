using System.Collections.Generic;

namespace GameLogics.Shared.Models.Configs {
	public class Config {
		public string Version { get; set; }
		
		public Dictionary<string, ItemConfig> Items { get; set; } = new Dictionary<string, ItemConfig>();
		public Dictionary<string, UnitConfig> Units { get; set; } = new Dictionary<string, UnitConfig>();

		public Config AddItem(string desc, ItemConfig item) {
			Items.Add(desc, item);
			return this;
		}

		public Config AddUnit(string desc, UnitConfig unit) {
			Units.Add(desc, unit);
			return this;
		}
	}
}