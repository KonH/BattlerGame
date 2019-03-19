using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class StartLevelCommandTest : BaseCommandTest<StartLevelCommand> {
		public StartLevelCommandTest() {
			_config
				.AddUnit("unit_desc",  new UnitConfig(1))
				.AddUnit("enemy_desc", new UnitConfig(1))
				.AddLevel("level_desc", new LevelConfig { EnemyDescriptors = { "enemy_desc" } });
			_state
				.AddUnit(new UnitState("unit_desc", 1).WithId("unit_id"));
		}

		string       LevelDesc    => "level_desc";
		List<string> PlayersUnits => new List<string> { "unit_id" };

		[Fact]
		void IsStateIsNullBefore() {
			Assert.Null(_state.Level);
		}
		
		[Fact]
		void CantStartWithInvalidParams() {
			IsInvalid(new StartLevelCommand(null, PlayersUnits));
			IsInvalid(new StartLevelCommand(LevelDesc, null));
		}

		[Fact]
		void CantStartUnknownLevel() {
			IsInvalid(new StartLevelCommand("unknown_desc", PlayersUnits));
		}

		[Fact]
		void CantStartWithoutUnits() {
			IsInvalid(new StartLevelCommand(LevelDesc, new List<string>()));
		}
		
		[Fact]
		void CantStartWithUnknownUnits() {
			IsInvalid(new StartLevelCommand(LevelDesc, new List<string> { "other_unit" }));
		}

		[Fact]
		void IsLevelStarted() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			Assert.NotNull(_state.Level);
			Assert.Equal(LevelDesc, _state.Level.Descriptor);
		}

		[Fact]
		void IsLevelContainsPlayerUnits() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));

			Assert.Contains(_state.Level.PlayerUnits, u => u.Id == "unit_id");
		}

		[Fact]
		void IsPlayerUnitsRemoved() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			Assert.False(_state.Units.ContainsKey("unit_id"));
		}
		
		[Fact]
		void IsLevelContainsEnemyUnits() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));

			Assert.Contains(_state.Level.EnemyUnits, u => u.Descriptor == "enemy_desc");
		}
		
		[Fact]
		void IsEnemyUnitsHasFakeIds() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			var units = _state.Level.EnemyUnits;
			Assert.NotEmpty(units);
			for ( var i = 0; i < units.Count; i++ ) {
				Assert.Equal(i.ToString(), units[i].Id);
			}
		}
	}
}