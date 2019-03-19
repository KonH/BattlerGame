using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public class AddItemCommandTest : BaseCommandTest<AddItemCommand> {
		public AddItemCommandTest() {
			_config.AddItem("desc", new ItemConfig());
		}
		
		[Fact]
		void CantAddInvalidItem() {
			IsInvalid(new AddItemCommand(null, "desc"));
			IsInvalid(new AddItemCommand("id", null));
			IsInvalid(new AddItemCommand(null, null));
		}
		
		[Fact]
		void CantAddAlreadyExistingItem() {
			var item = new ItemState("desc").WithNewId();
			_state.AddItem(item);
			
			IsInvalid(new AddItemCommand(item.Id, "desc"));
		}
		
		[Fact]
		void CantAddUnknownItem() {
			IsInvalid(new AddItemCommand("id", "unknown_desc"));
		}
		
		[Fact]
		void ItemWasAdded() {
			var item = new ItemState("desc").WithNewId();
			
			Execute(new AddItemCommand(item.Id, item.Descriptor));
			
			Assert.True(_state.Items.ContainsKey(item.Id));
			Assert.Equal(item.Descriptor, _state.Items[item.Id].Descriptor);
		}

		[Fact]
		void CantBeCalledDirectly() {
			var item = new ItemState("desc").WithNewId();
			
			IsInvalidOnServer(new AddItemCommand(item.Id, item.Descriptor));
		}
	}
}