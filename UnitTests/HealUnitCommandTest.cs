using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class HealUnitCommandTest : BaseCommandTest<HealUnitCommand> {
		ulong _unitId;

		public HealUnitCommandTest() {
			_unitId = _state.NewEntityId();
			_config.AddUnit("unit_desc", new UnitConfig(1, 2));
			_state.AddUnit(new UnitState("unit_desc", 1).WithId(_unitId));
		}

		[Fact]
		void CantHealUnknownUnit() {
			IsInvalid(new HealUnitCommand(0));
		}

		[Fact]
		void CantHealUnitWithoutDesc() {
			_state.Units[_unitId].Descriptor = "";
			
			IsInvalid(new HealUnitCommand(_unitId));
		}

		[Fact]
		void UnitHealthRestored() {
			Execute(new HealUnitCommand(_unitId));
			
			Assert.Equal(2, _state.Units[_unitId].Health);
		}

		[Fact]
		void CantBeCalledDirectly() {
			IsInvalidOnServer(new HealUnitCommand(_unitId));
		}
	}
}