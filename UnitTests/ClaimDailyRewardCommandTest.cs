using System;
using System.Linq;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class ClaimDailyRewardCommandTest : BaseCommandTest<ClaimDailyRewardCommand> {
		DateTime _origin = new DateTime(2000, 1, 1, 14, 0, 0);
		DateTime _day1;
		DateTime _day2;
		DateTime _day3;

		public ClaimDailyRewardCommandTest() {
			_day1 = _origin.AddDays(1).AddHours(1);
			_day2 = _origin.AddDays(2).AddHours(12);
			_day3 = _origin.AddDays(3).AddHours(23);

			_config.AddItem("item", new WeaponConfig(1));
			_config.AddUnit("unit", new UnitConfig(1, 1));
			_config.AddReward("reward_1", new RewardConfig { Resources = new RewardInterval(1, 1) });
			_config.AddReward("reward_2", new RewardConfig { Items = new RewardInterval(1, 1) });
			_config.AddReward("reward_3", new RewardConfig { Units = new RewardInterval(1, 1) });
			_config.DailyRewards = new string[] { "reward_1", "reward_2", "reward_3" };

			_state.Time.LastSyncTime = _origin;
			_state.DailyReward.LastClaimDate = _origin;
		}

		[Fact]
		void CantBeClaimedIfNoDaysPassed() {
			_state.DailyReward.LastClaimDate = _origin.AddMinutes(-10);

			IsInvalid(new ClaimDailyRewardCommand());
		}

		[Fact]
		void CanBeClaimedIfOneDayPassed() {
			IsValid(new ClaimDailyRewardCommand(), _day1 - _origin);
		}

		[Fact]
		void IsDayUpdated() {
			Execute(new ClaimDailyRewardCommand(), offset: _day1 - _origin);

			Assert.Equal(_day1, _state.DailyReward.LastClaimDate);
		}

		[Fact]
		void IsStreakUpdated() {
			Execute(new ClaimDailyRewardCommand(), offset: _day1 - _origin);

			Assert.Equal(1, _state.DailyReward.Streak);
		}

		[Fact]
		void IsStreakNotPersist() {
			Execute(new ClaimDailyRewardCommand(), offset: _day1 - _origin);
			Execute(new ClaimDailyRewardCommand(), offset: _day2 - _origin);
			Execute(new ClaimDailyRewardCommand(), offset: _day3 - _origin);

			Assert.Equal(0, _state.DailyReward.Streak);
		}

		[Fact]
		void IsStreakDiscartedIfDaySkipped() {
			Execute(new ClaimDailyRewardCommand(), offset: _day1 - _origin);
			Execute(new ClaimDailyRewardCommand(), offset: _day2.AddDays(1) - _origin);

			Assert.Equal(0, _state.DailyReward.Streak);
		}

		[Fact]
		void IsProperRewardsGiven() {
			_state.DailyReward.LastClaimDate = default;

			Execute(new ClaimDailyRewardCommand(), offset: _day1 - _origin);
			Assert.Equal(1, _state.Resources.Values.Sum());

			Execute(new ClaimDailyRewardCommand(), offset: _day2 - _origin);
			Assert.Single(_state.Items);

			Execute(new ClaimDailyRewardCommand(), offset: _day3 - _origin);
			Assert.Single(_state.Units);
		}
	}
}
