using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class RemoveUnitCommandTest : BaseCommandTest<RemoveUnitCommand> {
		[Fact]
		void CantRemoveInvalidUnit() {
			IsInvalid(new RemoveUnitCommand(null));
		}
		
		[Fact]
		void CantRemoveNotExistingUnit() {
			IsInvalid(new RemoveUnitCommand("non_existed_id"));
		}
		
		[Fact]
		void UnitWasRemoved() {
			var item = new UnitState("desc", 1).WithNewId();
			_state.AddUnit(item);
			
			Execute(new RemoveUnitCommand(item.Id));
			
			Assert.False(_state.Units.ContainsKey(item.Id));
		}
	}
}