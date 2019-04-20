using System.Collections.Generic;

namespace GameLogics.Shared.Model.Config {
	public sealed class LevelConfig {
		public List<string> EnemyDescriptors { get; } = new List<string>();
		public string       RewardLevel      { get; set; }
	}
}