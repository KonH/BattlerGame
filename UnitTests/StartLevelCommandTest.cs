using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class StartLevelCommandTest : BaseCommandTest<StartLevelCommand> {
		ulong _unitId;
		
		public StartLevelCommandTest() {
			_config
				.AddUnit("unit_desc",  new UnitConfig(1))
				.AddUnit("enemy_desc", new UnitConfig(1))
				.AddLevel("level_desc", new LevelConfig { EnemyDescriptors = { "enemy_desc" } });
			_unitId = NewId();
			_state
				.AddUnit(new UnitState("unit_desc", 1).WithId(_unitId));
		}

		string      LevelDesc    => "level_desc";
		List<ulong> PlayersUnits => new List<ulong> { _unitId };

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
			IsInvalid(new StartLevelCommand(LevelDesc, new List<ulong>()));
		}
		
		[Fact]
		void CantStartWithUnknownUnits() {
			IsInvalid(new StartLevelCommand(LevelDesc, new List<ulong> { InvalidId }));
		}
		
		[Fact]
		void CantStartWithDeadUnits() {
			_state.Units[_unitId].Health = 0;
			
			IsInvalid(new StartLevelCommand(LevelDesc, new List<ulong> { _unitId }));
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

			Assert.Contains(_state.Level.PlayerUnits, u => u.Id == _unitId);
		}

		[Fact]
		void IsPlayerUnitsRemoved() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			Assert.False(_state.Units.ContainsKey(_unitId));
		}
		
		[Fact]
		void IsLevelContainsEnemyUnits() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));

			Assert.Contains(_state.Level.EnemyUnits, u => u.Descriptor == "enemy_desc");
		}
		
		[Fact]
		void IsEnemyUnitsHasRealIds() {
			var startId = _state.EntityId;
			
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			var units = _state.Level.EnemyUnits;
			Assert.NotEmpty(units);
			for ( var i = 0; i < units.Count; i++ ) {
				Assert.Equal(startId + 1 + (ulong)i, units[i].Id);
			}
		}

		[Fact]
		void IsLevelStartedWithPlayerTurn() {
			Execute(new StartLevelCommand(LevelDesc, PlayersUnits));
			
			Assert.True(_state.Level.PlayerTurn);
		}
	}
}