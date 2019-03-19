using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class KillUnitCommandTest : BaseCommandTest<KillUnitCommand> {
		public KillUnitCommandTest() {
			_state.Level = new LevelState(
				"level_desc",
				new List<UnitState> { new UnitState("player_unit", 0).WithId("player_id") },
				new List<UnitState> { new UnitState("enemy_unit", 0).WithId("enemy_id") }
			);
		}
		
		[Fact]
		void CantKillIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new KillUnitCommand("player_id"));
		}

		[Fact]
		void CantKillIfHealthIsMoreThanZero() {
			_state.Level.PlayerUnits[0].Health = 1;
			
			IsInvalid(new KillUnitCommand("player_id"));
		}
		
		[Fact]
		void CantKillUnknownUnit() {
			IsInvalid(new KillUnitCommand("player_id_2"));
		}
		
		[Fact]
		void IsEnemyUnitWasRemoved() {
			Execute(new KillUnitCommand("enemy_id"));

			Assert.Empty(_state.Level.EnemyUnits);
		}

		[Fact]
		void IsPlayerUnitWasReturned() {
			Execute(new KillUnitCommand("player_id"));

			Assert.Empty(_state.Level.PlayerUnits);
			Assert.Contains(_state.Units.Values, u => u.Id == "player_id");
		}
		
		[Fact]
		void IfEnemyUnitsKilledLevelWillWon() {
			Produces<FinishLevelCommand>(new KillUnitCommand("enemy_id"), c => c.Win);
		}
		
		[Fact]
		void IfPlayerUnitsKilledLevelWillFailed() {
			Produces<FinishLevelCommand>(new KillUnitCommand("player_id"), c => !c.Win);
		}

		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new KillUnitCommand("enemy_id"));
		}
	}
}