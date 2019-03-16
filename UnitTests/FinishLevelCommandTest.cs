using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class FinishLevelCommandTest : BaseCommandTest<FinishLevelCommand> {
		public FinishLevelCommandTest() {
			_config
				.AddUnit("unit_desc",  new UnitConfig())
				.AddUnit("enemy_desc", new UnitConfig())
				.AddLevel("level_desc", new LevelConfig { EnemyDescriptors = { "enemy_desc" } });
			_state.Level = new LevelState(
				"level_desc", new List<UnitState> { new UnitState("unit_desc", 1).WithId("unit_id") }, new List<UnitState>()
			);
		}

		[Fact]
		void CantFinishLevelIfNotStarted() {
			_state.Level = null;
			
			IsInvalid(new FinishLevelCommand());
		}

		[Fact]
		void IsLevelStateCleared() {
			Execute(new FinishLevelCommand());
			
			Assert.Null(_state.Level);
		}

		[Fact]
		void IsPlayerUnitsReturned() {
			Execute(new FinishLevelCommand());
			
			Assert.True(_state.Units.ContainsKey("unit_id"));
		}
	}
}