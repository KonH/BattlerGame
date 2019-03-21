using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Services;
using Xunit;

namespace UnitTests {
	public class ConvertServiceTest {
		class TestCommand : BaseCommand {			
			protected override bool IsValid(GameState state, Config config) => true;
			protected override void Execute(GameState state, Config config, ICommandBuffer buffer) {}
		}
		
		ConvertService _service = new ConvertService();
		
		[Fact]
		void IsEntityIdIncreased() {
			var state = new GameState();
			var newId = state.NewEntityId();

			var newState = _service.DoubleConvert(state);
			
			Assert.Equal(newId, newState.EntityId);
		}

		[Fact]
		void IsCommandSerializedWithoutEnumerator() {
			var cmd = new TestCommand();
			
			var _ = _service.DoubleConvert(cmd);
			
			Assert.True(cmd.AsEnumerable().GetEnumerator().MoveNext());
		}
	}
}