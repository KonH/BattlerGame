using System;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class ClaimEventRewardCommandTest : BaseCommandTest<ClaimEventRewardCommand> {
		DateTime _startTime = new DateTime(2000, 1, 1, 14, 0, 0);
		TimeSpan _duration  = TimeSpan.FromDays(1);

		public ClaimEventRewardCommandTest() {
			_config
				.AddItem("item", new WeaponConfig(1))
				.AddReward("reward", new RewardConfig { Items = new RewardInterval(1, 1) })
				.AddLevel("event_level_0", new LevelConfig())
				.AddEvent("event", new EventConfig { Scope = "event_level", RewardLevel = "reward", StartTime = _startTime, Duration = _duration });

			_state.Time.LastSyncTime = _startTime;
			_state.Progress["event_level"] = 1;
		}

		[Fact]
		void CantClaimFromThePast() {
			_state.Time.LastSyncTime = _state.Time.LastSyncTime.AddMinutes(-1);

			IsInvalid(new ClaimEventRewardCommand("event"));
		}

		[Fact]
		void CantClaimIfTimePassed() {
			_state.Time.LastSyncTime = _state.Time.LastSyncTime.Add(_duration).AddSeconds(1);

			IsInvalid(new ClaimEventRewardCommand("event"));
		}

		[Fact]
		void CantClaimIfNotEnoughProgress() {
			_state.Progress["event_level"] = 0;

			IsInvalid(new ClaimEventRewardCommand("event"));
		}

		[Fact]
		void CantClaimSecondTime() {
			Execute(new ClaimEventRewardCommand("event"));

			IsInvalid(new ClaimEventRewardCommand("event"));
		}

		[Fact]
		void IsProperRewardGiven() {
			Execute(new ClaimEventRewardCommand("event"));

			Assert.Single(_state.Items);
		}

		[Fact]
		void IsScopeCleared() {
			Execute(new ClaimEventRewardCommand("event"));

			Assert.False(_state.Progress.ContainsKey("event_level"));
		}
	}
}
