using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public class TakeOffItemCommandTest : BaseCommandTest<TakeOffItemCommand> {
		ulong _unitId;
		ulong _itemId;
		
		public TakeOffItemCommandTest() {
			_unitId = NewId();
			_itemId = NewId();
			_config.AddItem("item_desc", new WeaponConfig());
			_state.AddUnit(new UnitState("unit_desc", 1).WithId(_unitId));
			_state.Units[_unitId].Items.Add(new ItemState("item_desc").WithId(_itemId));
		}

		[Fact]
		void CantTakeoffUnknownItem() {
			IsInvalid(new TakeOffItemCommand(InvalidId, _unitId));
		}
		
		[Fact]
		void CantTakeoffFromUnknownUnit() {
			IsInvalid(new TakeOffItemCommand(_itemId, InvalidId));
		}
		
		[Fact]
		void ItemWasRemovedFromUnit() {
			Execute(new TakeOffItemCommand(_itemId, _unitId));
			
			Assert.DoesNotContain(_state.Units[_unitId].Items, it => (it.Id == _itemId));
		}
		
		[Fact]
		void ItemWasAddedToInventory() {
			Execute(new TakeOffItemCommand(_itemId, _unitId));
			
			Assert.True(_state.Items.ContainsKey(_itemId));
		}
	}
}