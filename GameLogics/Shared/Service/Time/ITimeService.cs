using System;

namespace GameLogics.Shared.Service.Time {
	public interface ITimeService {
		DateTime RealTime { get; }
	}
}
