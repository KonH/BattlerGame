using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public class SpendResoucesCommandTest {
		[Fact]
		void CantSpendUnknownResource() {
			Assert.False(new SpendResourceCommand(Resource.Unknown, 1).IsValid);
		}
		
		[Fact]
		void CantSpendInvalidCount() {
			Assert.False(new SpendResourceCommand(Resource.Coins, 0).IsValid);
			Assert.False(new SpendResourceCommand(Resource.Coins, -1).IsValid);
		}
		
		[Fact]
		void ResourcesWasSpend() {
			var gs = new GameState();
			gs.Resources.Add(Resource.Coins, 1);
			var cmd = new SpendResourceCommand(Resource.Coins, 1);
			cmd.Execute(gs);
			Assert.Equal(0, gs.Resources.GetOrDefault(Resource.Coins));
		}
	}
}