using GameLogics.Shared.Command;
using GameLogics.Shared.Model;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public sealed class AddResoucesCommandTest : BaseCommandTest<AddResourceCommand> {
		[Fact]
		void CantAddUnknownResource() {
			IsInvalid(new AddResourceCommand(Resource.Unknown, 1));
		}
		
		[Fact]
		void CantAddInvalidCount() {
			IsInvalid(new AddResourceCommand(Resource.Coins, 0));
			IsInvalid(new AddResourceCommand(Resource.Coins, -1));
		}
		
		[Fact]
		void ResourceWasAdded() {
			Execute(new AddResourceCommand(Resource.Coins, 1));

			Assert.Equal(1, _state.Resources.GetOrDefault(Resource.Coins));
		}

		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new AddResourceCommand(Resource.Coins, 1));
		}
	}
}