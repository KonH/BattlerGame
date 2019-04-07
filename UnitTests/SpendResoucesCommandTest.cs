using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Utils;
using Xunit;

namespace UnitTests {
	public sealed class SpendResoucesCommandTest : BaseCommandTest<SpendResourceCommand> {
		[Fact]
		void CantSpendUnknownResource() {
			IsInvalid(new SpendResourceCommand(Resource.Unknown, 1));
		}
		
		[Fact]
		void CantSpendInvalidCount() {
			IsInvalid(new SpendResourceCommand(Resource.Coins, 0));
			IsInvalid(new SpendResourceCommand(Resource.Coins, -1));
		}
		
		[Fact]
		void ResourcesWasSpend() {
			_state.Resources.Add(Resource.Coins, 1);
			
			Execute(new SpendResourceCommand(Resource.Coins, 1));
			
			Assert.Equal(0, _state.Resources.GetOrDefault(Resource.Coins));
		}
	}
}