using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using Xunit;

namespace UnitTests {
	public sealed class RemoveItemCommandTest : BaseCommandTest<RemoveItemCommand> {		
		[Fact]
		void CantRemoveNotExistingItem() {
			IsInvalid(new RemoveItemCommand(InvalidId));
		}
		
		[Fact]
		void ItemWasRemoved() {
			var item = new ItemState("desc").WithId(NewId());
			_state.AddItem(item);
			
			Execute(new RemoveItemCommand(item.Id));
			
			Assert.False(_state.Items.ContainsKey(item.Id));
		}
	}
}