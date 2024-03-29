using System.Collections.Generic;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using Xunit;
using System;

namespace UnitTests {
	public sealed class StartLevelCommandTest : BaseCommandTest<StartLevelCommand> {
		ulong _unitId;

		public StartLevelCommandTest() {
			_config
				.AddUnit("unit_desc", new UnitConfig(1, 1))
				.AddUnit("enemy_desc", new UnitConfig(1, 1))
				.AddLevel("level_0", new LevelConfig { EnemyDescriptors = { "enemy_desc" } })
				.AddLevel("farm_0", new LevelConfig { EnemyDescriptors = { "enemy_desc" } });
			_config.Farming.Add("farm", new FarmConfig { Interval = TimeSpan.FromSeconds(10) });
			_unitId = NewId();
			_state
				.AddUnit(new UnitState("unit_desc", 1).WithId(_unitId));
		}

		string      LevelDesc    => "level_0";
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
		void CantStartWithoutProgress() {
			_config.AddLevel("level_1", _config.Levels["level_0"]);
			
			IsInvalid(new StartLevelCommand("level_1", PlayersUnits));
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

		[Fact]
		void CantStartLevelWithDuplicatedUnits() {
			var units = PlayersUnits;
			units.Add(PlayersUnits[0]);
			
			IsInvalid(new StartLevelCommand(LevelDesc, units));
		}
		
		[Fact]
		void CantStartLevelWithMoreThanHardLimitPlayerUnits() {
			var units = PlayersUnits;
			var hardLimit = 4;
			for ( var i = 0; i < hardLimit; i++ ) {
				var id = _state.NewEntityId();
				_state.AddUnit(new UnitState("unit_desc", 1).WithId(id));
				units.Add(id);
			}
			
			IsInvalid(new StartLevelCommand(LevelDesc, units));
		}

		[Fact]
		void FarmingLevelCanBeStarted() {
			_state.Time.LastSyncTime = DateTime.MinValue.Add(TimeSpan.FromSeconds(12));

			IsValid(new StartLevelCommand("farm_0", PlayersUnits));
		}

		[Fact]
		void IsStartTimeForFarmLevelIsMarked() {
			var time = DateTime.MinValue.Add(TimeSpan.FromSeconds(12));
			_state.Time.LastSyncTime = time;

			Execute(new StartLevelCommand("farm_0", PlayersUnits));

			Assert.Equal(time, _state.Farming["farm"]);
		}

		[Fact]
		void FarmingLevelCantBeStartedSecondTimeIfNoEnoughTimePassed() {
			_state.Farming["farm"] = _state.Time.GetRealTime();

			IsInvalid(new StartLevelCommand("farm_0", PlayersUnits));
		}

		[Fact]
		void FarmingLevelCanBeStartedSecondTimeIfEnoughTimePassed() {
			_state.Time.LastSyncTime = DateTime.MinValue.Add(TimeSpan.FromSeconds(12));
			_state.Farming["farm"] = _state.Time.GetRealTime() - TimeSpan.FromSeconds(11);

			IsValid(new StartLevelCommand("farm_0", PlayersUnits));
		}
	}
}