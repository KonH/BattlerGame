using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class RemoveItemCommandTest : BaseCommandTest<RemoveItemCommand> {	
		[Fact]
		void CantRemoveInvalidItem() {
			IsInvalid(new RemoveItemCommand(null));
		}
		
		[Fact]
		void CantRemoveNotExistingItem() {
			IsInvalid(new RemoveItemCommand("non_existed_id"));
		}
		
		[Fact]
		void ItemWasRemoved() {
			var item = new ItemState("desc").WithNewId();
			_state.AddItem(item);
			
			Execute(new RemoveItemCommand(item.Id));
			
			Assert.False(_state.Items.ContainsKey(item.Id));
		}
	}
}