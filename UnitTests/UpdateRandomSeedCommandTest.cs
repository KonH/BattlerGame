using GameLogics.Server.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;
using Xunit;

namespace UnitTests {
	public sealed class UpdateRandomSeedCommandTest : BaseCommandTest<UpdateRandomSeedCommand> {
		[Fact]
		void InitialSeedIsRandomized() {
			_config.AddUnit("weak_unit", new UnitConfig(1, 1));
			
			var initService = new StateInitService();
			var state1 = initService.Init(new GameState(), _config);
			var state2 = initService.Init(new GameState(), _config);

			Assert.NotEqual(0, state1.Random.Seed);
			Assert.NotEqual(0, state2.Random.Seed);
			Assert.NotEqual(state1.Random.Seed, state2.Random.Seed);
		}

		[Fact]
		void IsRandomDifferentBasedOnSeed() {
			var state1 = new GameState();
			var state2 = new GameState { Random = { Seed = int.MaxValue - 1 } }; // int.MaxValue leads to equal behaviour with 0


			var random1 = state1.CreateRandom();
			var random2 = state2.CreateRandom();

			Assert.NotEqual(random1.Next(100), random2.Next(100));
		}

		[Fact]
		void IsRandomEqualsOnDifferentStates() {
			var state1 = new GameState();
			var state2 = new GameState();

			var random1 = state1.CreateRandom();
			var random2 = state2.CreateRandom();

			Assert.Equal(random1.Next(int.MaxValue), random2.Next(int.MaxValue));
		}

		[Fact]
		void IsSeedUpdated() {
			var oldOffset = _state.Random.Seed;

			Execute(new UpdateRandomSeedCommand());
			
			Assert.NotEqual(oldOffset, _state.Random.Seed);
		}
		
		[Fact]
		void IsRandomDifferentAfterOffsetUpdated() {
			var random1 = _state.CreateRandom();
			Execute(new UpdateRandomSeedCommand());
			var random2 = _state.CreateRandom();
			
			Assert.NotEqual(random1.Next(int.MaxValue), random2.Next(int.MaxValue));
		}
		
		[Fact]
		void CantBeRunDirectly() {
			IsInvalidOnServer(new UpdateRandomSeedCommand());
		}
	}
}