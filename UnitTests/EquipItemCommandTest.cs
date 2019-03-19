using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public class EquipItemCommandTest : BaseCommandTest<EquipItemCommand> {
		ulong _unitId;
		ulong _itemId;
		
		public EquipItemCommandTest() {
			_config.AddUnit("unit_desc", new UnitConfig(1));
			_config.AddItem("item_desc", new ItemConfig { Type = ItemType.Weapon });
			_unitId = NewId();
			_itemId = NewId();
			_state.AddUnit(new UnitState("unit_desc", 1).WithId(_unitId));
			_state.AddItem(new ItemState("item_desc").WithId(_itemId));
		}

		[Fact]
		void CantEquipUnknownItem() {
			IsInvalid(new EquipItemCommand(InvalidId, _unitId));
		}

		[Fact]
		void CantEquipToUnknownUnit() {
			IsInvalid(new EquipItemCommand(_itemId, InvalidId));
		}
		
		[Fact]
		void CantEquipItemWithoutDescription() {
			_state.Items[_itemId].Descriptor = "invalid";
			IsInvalid(new EquipItemCommand(_itemId, _unitId));
		}

		[Fact]
		void CantEquipToUnitWithoutDescription() {
			_state.Units[_unitId].Descriptor = "invalid";
			IsInvalid(new EquipItemCommand(_itemId, _unitId));
		}

		[Fact]
		void CantEquipItemWithSameType() {
			Execute(new EquipItemCommand(_itemId, _unitId));

			var id = NewId();
			_state.AddItem(new ItemState("item_desc").WithId(id));
			IsInvalid(new EquipItemCommand(id, _unitId));
		}
		
		[Fact]
		void ItemWasEquiped() {
			Execute(new EquipItemCommand(_itemId, _unitId));
			
			Assert.Contains(_state.Units[_unitId].Items, it => (it.Id == _itemId));
		}
		
		[Fact]
		void ItemWasRemovedFromInventory() {
			Execute(new EquipItemCommand(_itemId, _unitId));
			
			Assert.False(_state.Items.ContainsKey(_itemId));
		}
	}
}