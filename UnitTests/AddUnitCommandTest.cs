using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public sealed class AddUnitCommandTest : BaseCommandTest<AddUnitCommand> {
		public AddUnitCommandTest() {
			_config.AddUnit("desc", new UnitConfig(1, 1));
		}
		
		[Fact]
		void CantAddInvalidUnit() {
			IsInvalid(new AddUnitCommand(_state.EntityId, null));
		}
		
		[Fact]
		void CantAddAlreadyExistingUnit() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			_state.AddUnit(unit);
			
			IsInvalid(new AddUnitCommand(unit.Id, "desc"));
		}

		[Fact]
		void CantAddUnknownUnit() {
			IsInvalid(new AddUnitCommand(_state.EntityId, "unknown_desc"));
		}
		
		[Fact]
		void UnitWasAdded() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			
			Execute(new AddUnitCommand(unit.Id, unit.Descriptor));
			
			Assert.True(_state.Units.ContainsKey(unit.Id));
			Assert.Equal(unit.Descriptor, _state.Units[unit.Id].Descriptor);
		}
		
		[Fact]
		void UniHealthIsSet() {
			_config.Units["desc"].MaxHealth = new int[] { 2 };
			var id = _state.NewEntityId();
			
			Execute(new AddUnitCommand(id, "desc"));

			Assert.Equal(2, _state.Units[id].Health);
		}
		
		[Fact]
		void CantBeCalledDirectly() {
			var unit = new UnitState("desc", 1).WithId(NewId());
			
			IsInvalidOnServer(new AddUnitCommand(unit.Id, unit.Descriptor));
		}
	}
}