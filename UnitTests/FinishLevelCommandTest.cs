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
				.AddLevel("level_desc", new LevelConfig { EnemyDescriptors = { "enemy_desc" } });
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
	}
}