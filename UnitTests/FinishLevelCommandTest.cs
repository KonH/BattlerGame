using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class FinishLevelCommandTest : BaseCommandTest<FinishLevelCommand> {
		public FinishLevelCommandTest() {
			_config
				.AddUnit("unit_desc",  new UnitConfig(1))
				.AddUnit("enemy_desc", new UnitConfig(1))
				.AddLevel("level_desc", new LevelConfig {
					EnemyDescriptors = {
						"enemy_desc",
					},
					Reward = {
						Resources = { { Resource.Coins, 1 } },
						Items     = { "reward_item" },
						Units     = { "reward_unit" },
					}
				});
			_state.Level = new LevelState(
				"level_desc", new List<UnitState> { new UnitState("unit_desc", 1).WithId("unit_id") }, new List<UnitState>()
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
			
			Assert.True(_state.Units.ContainsKey("unit_id"));
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new FinishLevelCommand(true));
		}

		[Fact]
		void IsRewardsGainedIfLevelWon() {
			ProducesAll(
				new FinishLevelCommand(true),
				ic => {
					switch ( ic ) {
						case AddResourceCommand c: return (c.Kind == Resource.Coins) && (c.Count == 1);
						case AddItemCommand c:     return (c.Descriptor == "reward_item");
						case AddUnitCommand c:     return (c.Descriptor == "reward_unit");
						default:                   return false;
					}
				});
		}
		
		[Fact]
		void IsRewardsNotGainedIfLevelFailed() {
			ProducesNone(new FinishLevelCommand(false));
		}
	}
}