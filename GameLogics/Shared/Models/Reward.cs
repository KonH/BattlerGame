using System.Collections.Generic;

namespace GameLogics.Shared.Models {
	public sealed class Reward {
		public Dictionary<Resource, int> Resources { get; } = new Dictionary<Resource, int>();
		public List<string>              Items     { get; } = new List<string>();
		public List<string>              Units     { get; } = new List<string>();
	}
}