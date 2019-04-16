using System;

namespace GameLogics.Shared.Service.Time {
	public sealed class RealTimeService : ITimeService {
		public DateTime RealTime => DateTime.UtcNow;
	}
}
