using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class RemoveUnitCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantRemoveInvalidUnit() {
			Assert.False(new RemoveUnitCommand(null).IsValid(_state));
		}
		
		[Fact]
		void CantRemoveNotExistingUnit() {
			Assert.False(new RemoveUnitCommand("non_existed_id").IsValid(_state));
		}
		
		[Fact]
		void UnitWasRemoved() {
			var item = new UnitState("desc", 1).WithNewId();
			_state.AddUnit(item);
			new RemoveUnitCommand(item.Id).Execute(_state);
			Assert.False(_state.Units.ContainsKey(item.Id));
		}
	}
}