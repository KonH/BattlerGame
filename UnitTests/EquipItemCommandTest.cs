using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public class EquipItemCommandTest : BaseCommandTest<EquipItemCommand> {
		public EquipItemCommandTest() {
			_config.AddUnit("unit_desc", new UnitConfig(1));
			_config.AddItem("item_desc", new ItemConfig { Type = ItemType.Weapon });
			_state.AddUnit(new UnitState("unit_desc", 1).WithId("unit_id"));
			_state.AddItem(new ItemState("item_desc").WithId("item_id"));
		}
		
		[Fact]
		void CantEquipWithInvalidParams() {
			IsInvalid(new EquipItemCommand(null, "unit_id"));
			IsInvalid(new EquipItemCommand("item_id", null));
			IsInvalid(new EquipItemCommand(null, null));
		}

		[Fact]
		void CantEquipUnknownItem() {
			IsInvalid(new EquipItemCommand("unknown_id", "unit_id"));
		}

		[Fact]
		void CantEquipToUnknownUnit() {
			IsInvalid(new EquipItemCommand("item_id", "unknown_id"));
		}
		
		[Fact]
		void CantEquipItemWithoutDescription() {
			_state.Items["item_id"].Descriptor = "invalid";
			IsInvalid(new EquipItemCommand("item_id", "unit_id"));
		}

		[Fact]
		void CantEquipToUnitWithoutDescription() {
			_state.Units["unit_id"].Descriptor = "invalid";
			IsInvalid(new EquipItemCommand("item_id", "unit_id"));
		}

		[Fact]
		void CantEquipItemWithSameType() {
			Execute(new EquipItemCommand("item_id", "unit_id"));

			_state.AddItem(new ItemState("item_desc").WithId("another_item_id"));
			IsInvalid(new EquipItemCommand("another_item_id", "unit_id"));
		}
		
		[Fact]
		void ItemWasEquiped() {
			Execute(new EquipItemCommand("item_id", "unit_id"));
			
			Assert.Contains(_state.Units["unit_id"].Items, it => (it.Id == "item_id"));
		}
		
		[Fact]
		void ItemWasRemovedFromInventory() {
			Execute(new EquipItemCommand("item_id", "unit_id"));
			
			Assert.False(_state.Items.ContainsKey("item_id"));
		}
	}
}