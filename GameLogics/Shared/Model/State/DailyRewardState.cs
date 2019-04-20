using System;

namespace GameLogics.Shared.Model.State {
	public sealed class DailyRewardState {
		public DateTime LastClaimDate;
		public int      Streak;
	}
}
