using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class AddUnitCommandTest : BaseCommandTest<AddUnitCommand> {
		public AddUnitCommandTest() {
			_config.AddUnit("desc", new UnitConfig());
		}
		
		[Fact]
		void CantAddInvalidUnit() {
			IsInvalid(new AddUnitCommand(null, "desc", 1));
			IsInvalid(new AddUnitCommand("id", null, 1));
			IsInvalid(new AddUnitCommand(null, null, 1));
			IsInvalid(new AddUnitCommand("id", "desc", -1));
		}
		
		[Fact]
		void CantAddAlreadyExistingUnit() {
			var unit = new UnitState("desc", 1).WithNewId();
			_state.AddUnit(unit);
			
			IsInvalid(new AddUnitCommand(unit.Id, "desc", 1));
		}

		[Fact]
		void CantAddUnknownUnit() {
			IsInvalid(new AddUnitCommand("id", "unknown_desc", 1));
		}
		
		[Fact]
		void UnitWasAdded() {
			var unit = new UnitState("desc", 1).WithNewId();
			
			Execute(new AddUnitCommand(unit.Id, unit.Descriptor, 1));
			
			Assert.True(_state.Units.ContainsKey(unit.Id));
			Assert.Equal(unit.Descriptor, _state.Units[unit.Id].Descriptor);
			Assert.Equal(1, _state.Units[unit.Id].Health);
		}
	}
}