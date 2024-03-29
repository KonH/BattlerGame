using System.Collections.Generic;
using GameLogics.Shared;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class FinishLevelCommandTest : BaseCommandTest<FinishLevelCommand> {
		ulong _unitId;
		
		public FinishLevelCommandTest() {
			_config
				.AddItem("reward_item", new WeaponConfig())
				.AddUnit("reward_unit", new UnitConfig(1, 1))
				.AddUnit("unit_desc",  new UnitConfig {
						BaseDamage = new int[] { 1, 1 },
						MaxHealth  = new int[] { 2, 2 }
					}
				)
				.AddUnit("enemy_desc", new UnitConfig(1, 1, 10))
				.AddLevel("level_0", new LevelConfig {
					EnemyDescriptors = {
						"enemy_desc",
					},
					RewardLevel = "test_reward"
				})
				.AddReward("test_reward", new RewardConfig {
					Resources = { Min = 100, Max = 100 },
					Items     = { Min = 1, Max = 1 },
					Units     = { Min = 1, Max = 1 }
				});
			_config.UnitLevels = new int[] { 100 };
			_unitId = NewId();
			_state.Level = new LevelState(
				"level_0", new List<UnitState> { new UnitState("unit_desc", 2).WithId(_unitId) }, new List<UnitState>()
			);
		}

		[Fact]
		void CantFinishLevelIfNotStarted() {
			_state.Level = null;
			
			IsInvalid(new FinishLevelCommand(true));
		}

		[Fact]
		void IsLevelStateCleared() {
			Execute(new FinishLevelCommand(true));
			
			Assert.Null(_state.Level);
		}

		[Fact]
		void IsPlayerUnitsReturned() {
			Execute(new FinishLevelCommand(true));
			
			Assert.True(_state.Units.ContainsKey(_unitId));
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new FinishLevelCommand(true));
		}

		[Fact]
		void IsRewardsNotGainedIfLevelFailed() {
			ProducesNone(new FinishLevelCommand(false));
		}

		[Fact]
		void IsHealthIsntRestoredByDefault() {
			_state.Level.PlayerUnits[0].Health = 1;
			
			Execute(new FinishLevelCommand(false));
			
			Assert.Equal(1, _state.Units[_unitId].Health);
		}
		
		[Fact]
		void IsHealthRestoredIfFeatureSet() {
			_config.Features[Features.AutoHeal] = true;
			_state.Level.PlayerUnits[0].Health = 1;
			
			Execute(new FinishLevelCommand(false));
			
			Assert.Equal(2, _state.Units[_unitId].Health);
		}

		[Fact]
		void IsRewardsGiven() {
			Execute(new FinishLevelCommand(true));
			
			Assert.Single(_state.Items);
			Assert.Equal(2, _state.Units.Count);
			Assert.Equal(100, _state.Resources[Resource.Coins]);
		}

		[Fact]
		void IsProgressUpdated() {
			Execute(new FinishLevelCommand(true));
			
			Assert.Equal(1, _state.Progress["level"]);
		}
		
		[Fact]
		void IsProgressDontUpdatedTwice() {
			Execute(new FinishLevelCommand(true));
			
			_state.Units.Clear();
			_state.Level = new LevelState(
				"level_0", new List<UnitState> { new UnitState("unit_desc", 2).WithId(_unitId) }, new List<UnitState>()
			);
			Execute(new FinishLevelCommand(true));
			
			Assert.Equal(1, _state.Progress["level"]);
		}

		[Fact]
		void IsExperienceGainedOnWin() {
			Execute(new FinishLevelCommand(true));

			Assert.Equal(10, _state.Units[_unitId].Experience);
		}

		[Fact]
		void IsFinishedNormallyIfTooMuchExperience() {
			_config.Units["enemy_desc"].Experience = 100;
			_config.Levels["level_0"].EnemyDescriptors.Add("enemy_desc");

			Execute(new FinishLevelCommand(true));

			Assert.Equal(0, _state.Units[_unitId].Experience);
			Assert.Equal(1, _state.Units[_unitId].Level);
		}

		[Fact]
		void IsExperienceNotGainedOnLose() {
			Execute(new FinishLevelCommand(false));

			Assert.Equal(0, _state.Units[_unitId].Experience);
		}
	}
}