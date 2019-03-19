using System.Collections.Generic;

namespace GameLogics.Shared.Models.Configs {
	public class LevelConfig {
		public List<string> EnemyDescriptors { get; } = new List<string>();
		public RewardConfig Reward           { get; } = new RewardConfig();
	}
}