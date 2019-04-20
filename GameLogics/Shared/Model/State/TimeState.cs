using System;

namespace GameLogics.Shared.Model.State {
	public sealed class TimeState {
		// Time of last state download
		public DateTime LastSyncTime;
		// E.g. cheats
		public TimeSpan PersistentOffset;
		// Represents time from LastSyncTime, can be modified from external services
		public TimeSpan TempOffset;

		public DateTime GetRealTime() => LastSyncTime.Add(PersistentOffset).Add(TempOffset);
	}
}
