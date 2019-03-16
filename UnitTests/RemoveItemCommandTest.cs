using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class RemoveItemCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantRemoveInvalidItem() {
			Assert.False(new RemoveItemCommand(null).IsValid(_state));
		}
		
		[Fact]
		void CantRemoveNotExistingItem() {
			Assert.False(new RemoveItemCommand("non_existed_id").IsValid(_state));
		}
		
		[Fact]
		void ItemWasRemoved() {
			var item = new ItemState("desc").WithNewId();
			_state.AddItem(item);
			new RemoveItemCommand(item.Id).Execute(_state);
			Assert.False(_state.Items.ContainsKey(item.Id));
		}
	}
}