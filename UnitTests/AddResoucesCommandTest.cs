using System;
using GameLogics.Commands;
using GameLogics.Core;
using GameLogics.Utils;
using Xunit;

namespace UnitTests {
	public class AddResoucesCommandTest {
		[Fact]
		void CantAddUnknownResourse() {
			Assert.Throws<InvalidOperationException>(() => {
				new AddResourceCommand(Resource.Unknown, 1).Execute(new GameState());
			});
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