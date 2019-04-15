using System.Collections.Generic;

namespace GameLogics.Shared.Model.Config {
	public sealed class ConfigRoot {
		public string Version { get; set; }
		
		public Dictionary<string, BaseItemConfig>     Items      { get; }      = new Dictionary<string, BaseItemConfig>();
		public Dictionary<string, UnitConfig>         Units      { get; }      = new Dictionary<string, UnitConfig>();
		public int[]                                  UnitLevels { get; set; } = new int[0];
		public Dictionary<string, LevelConfig>        Levels     { get; }      = new Dictionary<string, LevelConfig>();
		public Dictionary<string, bool>               Features   { get; }      = new Dictionary<string, bool>();
		public Dictionary<string, List<RewardConfig>> Rewards    { get; }      = new Dictionary<string, List<RewardConfig>>();

		public ConfigRoot AddItem(string desc, BaseItemConfig item) {
			Items.Add(desc, item);
			return this;
		}

		public ConfigRoot AddUnit(string desc, UnitConfig unit) {
			Units.Add(desc, unit);
			return this;
		}

		public ConfigRoot AddLevel(string desc, LevelConfig level) {
			Levels.Add(desc, level);
			return this;
		}

		public ConfigRoot AddReward(string desc, RewardConfig reward) {
			if ( !Rewards.TryGetValue(desc, out var rewards) ) {
				rewards = new List<RewardConfig>();
				Rewards.Add(desc, rewards);
			}
			rewards.Add(reward);
			return this;
		}

		public bool IsFeatureEnabled(string feature) => Features.TryGetValue(feature, out var value) && value;
	}
}