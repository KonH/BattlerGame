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
			var unit = new UnitState("desc", 1).WithNewId();
			_state.AddUnit(unit);
			
			Execute(new RemoveUnitCommand(unit.Id));
			
			Assert.False(_state.Units.ContainsKey(unit.Id));
		}

		[Fact]
		void ItemsWasReleasedToInventory() {
			var item = new ItemState("item_desc").WithId("item_id");
			var unit = new UnitState("desc", 1).WithNewId();
			unit.Items.Add(item);
			_state.AddUnit(unit);
			
			Execute(new RemoveUnitCommand(unit.Id));
			
			Assert.True(_state.Items.ContainsKey(item.Id));
		}
	}
}