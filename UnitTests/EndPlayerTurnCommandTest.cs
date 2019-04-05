using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using Xunit;

namespace UnitTests {
	public class EndPlayerTurnCommandTest : BaseCommandTest<EndPlayerTurnCommand> {
		public EndPlayerTurnCommandTest() {
			_state.Level = new LevelState(
				"level_0", new List<UnitState>(), new List<UnitState>()
			);
			_state.Level.PlayerTurn = true;
		}
		
		[Fact]
		void CantEndTurnIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new EndPlayerTurnCommand());
		}
		
		[Fact]
		void CantChangeTurnInsideEnemyTurn() {
			_state.Level.PlayerTurn = false;
			
			IsInvalid(new EndPlayerTurnCommand());
		}

		[Fact]
		void IsTurnChanged() {
			Execute(new EndPlayerTurnCommand());
			
			Assert.False(_state.Level.PlayerTurn);
		}

		[Fact]
		void IsMovedUnitsCleared() {
			_state.Level.MovedUnits.Add(0);
			
			Execute(new EndPlayerTurnCommand());
			
			Assert.Empty(_state.Level.MovedUnits);
		}
	}
}