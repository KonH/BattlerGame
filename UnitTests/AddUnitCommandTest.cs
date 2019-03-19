using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class AddUnitCommandTest : BaseCommandTest<AddUnitCommand> {
		public AddUnitCommandTest() {
			_config.AddUnit("desc", new UnitConfig(1));
		}
		
		[Fact]
		void CantAddInvalidUnit() {
			IsInvalid(new AddUnitCommand(_state.EntityId, null, 1));
			IsInvalid(new AddUnitCommand(_state.EntityId, "desc", -1));
		}
		
		[Fact]
		void CantAddAlreadyExistingUnit() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			_state.AddUnit(unit);
			
			IsInvalid(new AddUnitCommand(unit.Id, "desc", 1));
		}

		[Fact]
		void CantAddUnknownUnit() {
			IsInvalid(new AddUnitCommand(_state.EntityId, "unknown_desc", 1));
		}
		
		[Fact]
		void UnitWasAdded() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			
			Execute(new AddUnitCommand(unit.Id, unit.Descriptor, 1));
			
			Assert.True(_state.Units.ContainsKey(unit.Id));
			Assert.Equal(unit.Descriptor, _state.Units[unit.Id].Descriptor);
			Assert.Equal(1, _state.Units[unit.Id].Health);
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			
			IsInvalidOnServer(new AddUnitCommand(unit.Id, unit.Descriptor, 1));
		}
	}
}