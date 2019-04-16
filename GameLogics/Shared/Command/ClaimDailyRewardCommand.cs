using System;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Command {
	public sealed class ClaimDailyRewardCommand : ICommand {
		public bool IsValid(GameState state, ConfigRoot config) {
			if ( GetDaysBetween(state) < 1 ) {
				return false;
			} 
			return true;
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var days = GetDaysBetween(state);
			var maxStreaks = config.DailyRewards.Length;
			var isNeedToDiscard = (days >= 2) || (state.DailyReward.Streak == (maxStreaks - 1));
			var newStreak = isNeedToDiscard ? 0 : (state.DailyReward.Streak + 1);
			state.DailyReward.LastClaimDate = state.Time.GetRealTime();
			state.DailyReward.Streak = newStreak;
			var rewardLevel = config.DailyRewards[newStreak];
			RewardLogic.AppendReward(rewardLevel, state, config, buffer);
		}

		double GetDaysBetween(GameState state) {
			var oldDate = state.DailyReward.LastClaimDate;
			var newDate = state.Time.GetRealTime();
			return (newDate - oldDate).TotalDays;
		}

		public override string ToString() {
			return $"{nameof(ClaimDailyRewardCommand)}";
		}
	}
}
