using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using Xunit;

namespace UnitTests {
	public sealed class RemoveUnitCommandTest : BaseCommandTest<RemoveUnitCommand> {		
		[Fact]
		void CantRemoveNotExistingUnit() {
			IsInvalid(new RemoveUnitCommand(InvalidId));
		}
		
		[Fact]
		void UnitWasRemoved() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			_state.AddUnit(unit);
			
			Execute(new RemoveUnitCommand(unit.Id));
			
			Assert.False(_state.Units.ContainsKey(unit.Id));
		}

		[Fact]
		void ItemsWasReleasedToInventory() {
			var item = new ItemState("item_desc").WithId(NewId());
			var unit = new UnitState("desc", 1).WithId(NewId());
			unit.Items.Add(item);
			_state.AddUnit(unit);
			
			Execute(new RemoveUnitCommand(unit.Id));
			
			Assert.True(_state.Items.ContainsKey(item.Id));
		}
	}
}