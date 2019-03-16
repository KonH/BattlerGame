using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public class SpendResoucesCommandTest {
		GameState _state = new GameState();
		
		[Fact]
		void CantSpendUnknownResource() {
			Assert.False(new SpendResourceCommand(Resource.Unknown, 1).IsValid(_state));
		}
		
		[Fact]
		void CantSpendInvalidCount() {
			Assert.False(new SpendResourceCommand(Resource.Coins, 0).IsValid(_state));
			Assert.False(new SpendResourceCommand(Resource.Coins, -1).IsValid(_state));
		}
		
		[Fact]
		void ResourcesWasSpend() {
			_state.Resources.Add(Resource.Coins, 1);
			var cmd = new SpendResourceCommand(Resource.Coins, 1);
			cmd.Execute(_state);
			Assert.Equal(0, _state.Resources.GetOrDefault(Resource.Coins));
		}
	}
}