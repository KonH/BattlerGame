using System.Collections.Generic;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using Xunit;

namespace UnitTests {
	public sealed class EndEnemyTurnCommandTest : BaseCommandTest<EndEnemyTurnCommand> {
		public EndEnemyTurnCommandTest() {
			_state.Level = new LevelState(
				"level_0", new List<UnitState>(), new List<UnitState>()
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