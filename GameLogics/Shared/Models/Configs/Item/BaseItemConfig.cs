using System.Collections.Generic;

namespace GameLogics.Shared.Models.Configs {
	public abstract class BaseItemConfig {
		public virtual ItemType Type { get; }

		public Dictionary<Resource, int>[] UpgradePrice { get; set; }
	}
}