using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public class AddResoucesCommandTest {
		[Fact]
		void CantRequestUnknownResource() {
			Assert.False(new AddResourceCommand(Resource.Unknown, 1).IsValid);
		}
		
		[Fact]
		void CantRequestInvalidCount() {
			Assert.False(new AddResourceCommand(Resource.Coins, 0).IsValid);
			Assert.False(new AddResourceCommand(Resource.Coins, -1).IsValid);
		}
		
		[Fact]
		void ResourcesWasAdded() {
			var gs = new GameState();
			Assert.Equal(0, gs.Resources.GetOrDefault(Resource.Coins));
			var cmd = new AddResourceCommand(Resource.Coins, 1);
			cmd.Execute(gs);
			Assert.True(gs.Resources.ContainsKey(Resource.Coins));
			Assert.Equal(1, gs.Resources[Resource.Coins]);
		}
	}
}