using GameLogics.Shared.Models;
using GameLogics.Shared.Services;
using Xunit;

namespace UnitTests {
	public class ConvertServiceTest {
		ConvertService _service = new ConvertService();
		
		[Fact]
		void IsEntityIdIncreased() {
			var state = new GameState();
			var newId = state.NewEntityId();

			var newState = _service.DoubleConvert(state);
			
			Assert.Equal(newId, newState.EntityId);
		}
	}
}