using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class KillUnitCommandTest : BaseCommandTest<KillUnitCommand> {
		ulong _playerId;
		ulong _enemyId;
		
		public KillUnitCommandTest() {
			_playerId = NewId();
			_enemyId = NewId();
			_state.Level = new LevelState(
				"level_desc",
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
		void IsEnemyUnitWasRemoved() {
			Execute(new KillUnitCommand(_enemyId));

			Assert.Empty(_state.Level.EnemyUnits);
		}

		[Fact]
		void IsPlayerUnitWasReturned() {
			Execute(new KillUnitCommand(_playerId));

			Assert.Empty(_state.Level.PlayerUnits);
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
	}
}