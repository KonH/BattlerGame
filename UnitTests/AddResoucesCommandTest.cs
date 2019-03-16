using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public class AddResoucesCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantAddUnknownResource() {
			Assert.False(new AddResourceCommand(Resource.Unknown, 1).IsValid(_state));
		}
		
		[Fact]
		void CantAddInvalidCount() {
			Assert.False(new AddResourceCommand(Resource.Coins, 0).IsValid(_state));
			Assert.False(new AddResourceCommand(Resource.Coins, -1).IsValid(_state));
		}
		
		[Fact]
		void ResourceWasAdded() {
			var cmd = new AddResourceCommand(Resource.Coins, 1);
			cmd.Execute(_state);
			Assert.Equal(1, _state.Resources.GetOrDefault(Resource.Coins));
		}
	}
}