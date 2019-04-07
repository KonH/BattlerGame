using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {	
	public sealed class AddItemCommandTest : BaseCommandTest<AddItemCommand> {
		public AddItemCommandTest() {
			_config.AddItem("desc", new WeaponConfig());
		}
		
		[Fact]
		void CantAddInvalidItem() {
			IsInvalid(new AddItemCommand(_state.EntityId, null));
		}
		
		[Fact]
		void CantAddAlreadyExistingItem() {
			var item = new ItemState("desc").WithId(NewId());
			_state.AddItem(item);
			
			IsInvalid(new AddItemCommand(item.Id, "desc"));
		}
		
		[Fact]
		void CantAddUnknownItem() {
			IsInvalid(new AddItemCommand(NewId(), "unknown_desc"));
		}
		
		[Fact]
		void ItemWasAdded() {
			var item = new ItemState("desc").WithId(NewId());
			
			Execute(new AddItemCommand(item.Id, item.Descriptor));
			
			Assert.True(_state.Items.ContainsKey(item.Id));
			Assert.Equal(item.Descriptor, _state.Items[item.Id].Descriptor);
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			var item = new ItemState("desc").WithId(NewId());
			
			IsInvalidOnServer(new AddItemCommand(item.Id, item.Descriptor));
		}
	}
}