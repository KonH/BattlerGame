using System.Collections.Generic;
using GameLogics.Shared;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class KillUnitCommandTest : BaseCommandTest<KillUnitCommand> {
		ulong _playerId;
		ulong _enemyId;
		
		public KillUnitCommandTest() {
			_config
				.AddUnit("player_unit", new UnitConfig(1, 1))
				.AddLevel("level_0", new LevelConfig { RewardLevel = "" })
				.AddReward("", new RewardConfig());
			_playerId = NewId();
			_enemyId = NewId();
			_state.Level = new LevelState(
				"level_0",
				new List<UnitState> { new UnitState("player_unit", 0).WithId(_playerId) },
				new List<UnitState> { new UnitState("enemy_unit", 0).WithId(_enemyId) }
			);
		}
		
		[Fact]
		void CantKillIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new KillUnitCommand(_playerId));
		}

		[Fact]
		void CantKillIfHealthIsMoreThanZero() {
			_state.Level.PlayerUnits[0].Health = 1;
			
			IsInvalid(new KillUnitCommand(_playerId));
		}
		
		[Fact]
		void CantKillUnknownUnit() {
			IsInvalid(new KillUnitCommand(InvalidId));
		}

		[Fact]
		void IsPlayerUnitWasReturned() {
			Execute(new KillUnitCommand(_playerId));

			Assert.Contains(_state.Units.Values, u => u.Id == _playerId);
		}
		
		[Fact]
		void IfEnemyUnitsKilledLevelWillWon() {
			Produces<FinishLevelCommand>(new KillUnitCommand(_enemyId), c => c.Win);
		}
		
		[Fact]
		void IfPlayerUnitsKilledLevelWillFailed() {
			Produces<FinishLevelCommand>(new KillUnitCommand(_playerId), c => !c.Win);
		}

		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new KillUnitCommand(_enemyId));
		}

		[Fact]
		void IsKilledPlayerUnitHealed() {
			_config.Features[Features.AutoHeal] = true;
			
			Execute(new KillUnitCommand(_playerId));
			
			Assert.Equal(1, _state.Units[_playerId].Health);
		}
	}
}