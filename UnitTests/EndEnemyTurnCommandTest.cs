using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using Xunit;

namespace UnitTests {
	public class EndEnemyTurnCommandTest : BaseCommandTest<EndEnemyTurnCommand> {
		public EndEnemyTurnCommandTest() {
			_state.Level = new LevelState(
				"level_desc", new List<UnitState>(), new List<UnitState>()
			);
		}
		
		[Fact]
		void CantEndTurnIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new EndEnemyTurnCommand());
		}
		
		[Fact]
		void CantChangeTurnInsidePlayerTurn() {
			_state.Level.PlayerTurn = true;
			
			IsInvalid(new EndEnemyTurnCommand());
		}
		
		[Fact]
		void IsTurnChanged() {
			Execute(new EndEnemyTurnCommand());
			
			Assert.True(_state.Level.PlayerTurn);
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new EndEnemyTurnCommand());
		}
		
		[Fact]
		void IsMovedUnitsCleared() {
			_state.Level.MovedUnits.Add(0);
			
			Execute(new EndEnemyTurnCommand());
			
			Assert.Empty(_state.Level.MovedUnits);
		}
	}
}