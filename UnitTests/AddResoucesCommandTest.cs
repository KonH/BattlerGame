using System;
using System.Threading.Tasks;
using GameLogics.Commands;
using GameLogics.Core;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using GameLogics.Utils;
using Xunit;

namespace UnitTests {
	public class AddResoucesCommandTest {
		[Fact]
		void CantRequestUnknownResource() {
			Assert.Throws<InvalidOperationException>(() => { new RequestResourceIntent(Resource.Unknown, 1); });
		}
		
		[Fact]
		void CantAddUnknownResourse() {
			Assert.Throws<InvalidOperationException>(() => {
				new AddResourceCommand(Resource.Unknown, 1).Execute(new GameState());
			});
		}

		[Fact]
		async Task CommandCreatedFromIntent() {
			var stateManager = new InMemoryGameStateManager(new GameState());
			var intentMapper = new DirectIntentToCommandMapper(stateManager);
			
			var intent = new RequestResourceIntent(Resource.Coins, 1);
			
			var response = await intentMapper.RequestCommandsFromIntent(intent);
			Assert.NotNull(response);
			Assert.True(response.Success);
			
			var commands = response.Commands;
			Assert.NotNull(commands);
			Assert.Collection(commands, cmd0 => {
				Assert.NotNull(cmd0);
				Assert.IsType<AddResourceCommand>(cmd0);
			});

			var addResourceCommand = (AddResourceCommand)commands[0];
			Assert.Equal(intent.Kind, addResourceCommand.Kind);
			Assert.Equal(intent.Count, addResourceCommand.Count);
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