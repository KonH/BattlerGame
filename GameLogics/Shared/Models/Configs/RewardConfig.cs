using System.Collections.Generic;

namespace GameLogics.Shared.Models.Configs {
	public class RewardConfig {
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();
		public List<string>              Items     { get; } = new List<string>();
		public List<string>              Units     { get; } = new List<string>();
	}
}