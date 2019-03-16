using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using Xunit;

namespace UnitTests {
	public class AddItemCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantAddInvalidItem() {
			Assert.False(new AddItemCommand(null, "desc").IsValid(_state));
			Assert.False(new AddItemCommand("id", null).IsValid(_state));
			Assert.False(new AddItemCommand(null, null).IsValid(_state));
		}
		
		[Fact]
		void CantAddAlreadyExistingItem() {
			var item = new ItemState("desc").WithNewId();
			_state.AddItem(item);
			Assert.False(new AddItemCommand(item.Id, "desc").IsValid(_state));
		}
		
		[Fact]
		void ItemWasAdded() {
			var item = new ItemState("desc").WithNewId();
			new AddItemCommand(item.Id, item.Descriptor).Execute(_state);
			Assert.True(_state.Items.ContainsKey(item.Id));
			Assert.Equal(item.Descriptor, _state.Items[item.Id].Descriptor);
		}
	}
}