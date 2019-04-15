using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Service;
using GameLogics.Server.Repository.Config;
using Xunit;

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
			var config = new ConfigRoot();
			
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
			var config = new ConfigRoot().AddItem("desc", new WeaponConfig());
			
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