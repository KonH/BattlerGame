using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class AddUnitCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantAddInvalidUnit() {
			Assert.False(new AddUnitCommand(null, "desc", 1).IsValid(_state));
			Assert.False(new AddUnitCommand("id", null, 1).IsValid(_state));
			Assert.False(new AddUnitCommand(null, null, 1).IsValid(_state));
			Assert.False(new AddUnitCommand("id", "desc", -1).IsValid(_state));
		}
		
		[Fact]
		void CantAddAlreadyExistingUnit() {
			var item = new UnitState("desc", 1).WithNewId();
			_state.AddUnit(item);
			Assert.False(new AddUnitCommand(item.Id, "desc", 1).IsValid(_state));
		}
		
		[Fact]
		void UnitWasAdded() {
			var item = new UnitState("desc", 1).WithNewId();
			new AddUnitCommand(item.Id, item.Descriptor, 1).Execute(_state);
			Assert.True(_state.Units.ContainsKey(item.Id));
			Assert.Equal(item.Descriptor, _state.Units[item.Id].Descriptor);
			Assert.Equal(1, _state.Units[item.Id].Health);
		}
	}
}