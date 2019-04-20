using System;

namespace GameLogics.Shared.Model.Config {
	public sealed class EventConfig {
		public string   Scope       { get; set; }
		public string   RewardLevel { get; set; }
		public DateTime StartTime   { get; set; }
		public TimeSpan Duration    { get; set; }
	}
}
