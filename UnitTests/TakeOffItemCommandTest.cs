using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public class TakeOffItemCommandTest : BaseCommandTest<TakeOffItemCommand> {
		public TakeOffItemCommandTest() {
			_config.AddItem("item_desc", new ItemConfig { Type = ItemType.Weapon });
			_state.AddUnit(new UnitState("unit_desc", 1).WithId("unit_id"));
			_state.Units["unit_id"].Items.Add(new ItemState("item_desc").WithId("item_id"));
		}
		
		[Fact]
		void CantTakeOffWithInvalidParams() {
			IsInvalid(new TakeOffItemCommand(null, "unit_id"));
			IsInvalid(new TakeOffItemCommand("item_id", null));
			IsInvalid(new TakeOffItemCommand(null, null));
		}

		[Fact]
		void CantTakeoffUnknownItem() {
			IsInvalid(new TakeOffItemCommand("unknown_id", "unit_id"));
		}
		
		[Fact]
		void CantTakeoffFromUnknownUnit() {
			IsInvalid(new TakeOffItemCommand("item_id", "unknown_id"));
		}
		
		[Fact]
		void ItemWasRemovedFromUnit() {
			Execute(new TakeOffItemCommand("item_id", "unit_id"));
			
			Assert.DoesNotContain(_state.Units["unit_id"].Items, it => (it.Id == "item_id"));
		}
		
		[Fact]
		void ItemWasAddedToInventory() {
			Execute(new TakeOffItemCommand("item_id", "unit_id"));
			
			Assert.True(_state.Items.ContainsKey("item_id"));
		}
	}
}