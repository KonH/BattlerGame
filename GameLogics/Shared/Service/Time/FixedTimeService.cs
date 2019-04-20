using System;

namespace GameLogics.Shared.Service.Time {
	public sealed class FixedTimeService : ITimeService {
		public DateTime FixedTime { get; set; }

		public DateTime RealTime => FixedTime;
	}
}
