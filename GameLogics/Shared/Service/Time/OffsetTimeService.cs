using System;

namespace GameLogics.Shared.Service.Time {
	public sealed class OffsetTimeService {
		readonly ITimeService _source;

		DateTime _origin;

		public OffsetTimeService(ITimeService source) {
			_source = source;
		}

		public void MarkOrigin() {
			_origin = _source.RealTime;
		}

		public TimeSpan Offset => _source.RealTime - _origin;
	}
}
