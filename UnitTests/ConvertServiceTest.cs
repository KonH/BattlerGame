using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;
using Xunit;
using GameLogics.Server.Repositories.Configs;

namespace UnitTests {
	public sealed class ConvertServiceTest {
		ConvertService _service = new ConvertService();

		[Fact]
		void IsStateSerialized() {
			var state = new GameState();
			
			var newState = _service.DoubleConvert(state);
			
			Assert.NotNull(newState);
		}

		[Fact]
		void IsConfigSerialized() {
			var config = new Config();
			
			var newConfig = _service.DoubleConvert(config);
			
			Assert.NotNull(newConfig);
		}
		
		[Fact]
		void IsEntityIdIncreased() {
			var state = new GameState();
			var newId = state.NewEntityId();

			var newState = _service.DoubleConvert(state);
			
			Assert.Equal(newId, newState.EntityId);
		}

		[Fact]
		void IsWeaponConfigSerialized() {
			var config = new Config().AddItem("desc", new WeaponConfig());
			
			var newConfig = _service.DoubleConvert(config);
			
			Assert.True(newConfig.Items.ContainsKey("desc"));
			Assert.NotNull(newConfig.Items["desc"]);
		}

		[Fact]
		void IsDefaultConfigSerialized() {
			var repo = new FileConfigRepository(_service, "../../../../UnityClient/Assets/Resources/Config.json");

			Assert.NotNull(repo.Get());
		}
	}
}