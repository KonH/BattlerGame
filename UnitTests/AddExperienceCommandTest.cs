using GameLogics.Shared.Command;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;
using Xunit;

namespace UnitTests {
	public sealed class AddExperienceCommandTest : BaseCommandTest<AddExperienceCommand> {
		readonly ulong _id;

		public AddExperienceCommandTest() {
			_config.AddUnit("unit", new UnitConfig {
				BaseDamage = new int[] { 1, 2, 3 },
				MaxHealth  = new int[] { 1, 2, 3 }
			});
			_id = _state.NewEntityId();
			_config.UnitLevels = new int[] { 100, 200 };
			var unit = new UnitState("unit", 1).WithId(_id);
			_state.AddUnit(unit);
		}

		[Fact]
		void CantAddExperienceToUnknownUnit() {
			IsInvalid(new AddExperienceCommand(ulong.MaxValue, 10));
		}

		[Fact]
		void CantAddInvalidExperienceAmount() {
			IsInvalid(new AddExperienceCommand(_id, -1));
			IsInvalid(new AddExperienceCommand(_id, 0));
		}

		[Fact]
		void CantAddOnLastLevel() {
			_state.Units[_id].Level = 2;

			IsInvalid(new AddExperienceCommand(_id, 10));
		}

		[Fact]
		void CantBeRunDirectly() {
			IsInvalidOnServer(new AddExperienceCommand(_id, 10));
		}

		[Fact]
		void IsExperienceIncreased() {
			Execute(new AddExperienceCommand(_id, 10));

			Assert.Equal(10, _state.Units[_id].Experience);
		}

		[Fact]
		void IsLevelUpProduced() {
			Produces<LevelUpCommand>(new AddExperienceCommand(_id, 100));
		}

		[Fact]
		void CanLevelUpSeveralSteps() {
			Execute(new AddExperienceCommand(_id, 300));

			Assert.Equal(2, _state.Units[_id].Level);
		}
	}
}
