using System.Collections.Generic;

namespace GameLogics.Shared.Models.Configs {
	public class Config {
		public string Version { get; set; }
		
		public Dictionary<string, IItemConfig> Items    { get; } = new Dictionary<string, IItemConfig>();
		public Dictionary<string, UnitConfig>  Units    { get; } = new Dictionary<string, UnitConfig>();
		public Dictionary<string, LevelConfig> Levels   { get; } = new Dictionary<string, LevelConfig>();
		public Dictionary<string, bool>        Features { get; } = new Dictionary<string, bool>();

		public Config AddItem(string desc, IItemConfig item) {
			Items.Add(desc, item);
			return this;
		}

		public Config AddUnit(string desc, UnitConfig unit) {
			Units.Add(desc, unit);
			return this;
		}

		public Config AddLevel(string desc, LevelConfig level) {
			Levels.Add(desc, level);
			return this;
		}

		public bool IsFeatureEnabled(string feature) => Features.TryGetValue(feature, out var value) && value;
	}
}