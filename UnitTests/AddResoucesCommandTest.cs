using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public class AddResoucesCommandTest {
		[Fact]
		void CantAddUnknownResource() {
			Assert.False(new AddResourceCommand(Resource.Unknown, 1).IsValid);
		}
		
		[Fact]
		void CantAddInvalidCount() {
			Assert.False(new AddResourceCommand(Resource.Coins, 0).IsValid);
			Assert.False(new AddResourceCommand(Resource.Coins, -1).IsValid);
		}
		
		[Fact]
		void ResourceWasAdded() {
			var gs = new GameState();
			var cmd = new AddResourceCommand(Resource.Coins, 1);
			cmd.Execute(gs);
			Assert.Equal(1, gs.Resources.GetOrDefault(Resource.Coins));
		}
	}
}