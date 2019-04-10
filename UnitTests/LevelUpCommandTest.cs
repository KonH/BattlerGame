using GameLogics.Shared.Commands;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;
using Xunit;

namespace UnitTests {
	public sealed class LevelUpCommandTest : BaseCommandTest<LevelUpCommand> {
		readonly ulong _id;

		public LevelUpCommandTest() {
			_config.AddUnit("unit", new UnitConfig {
				BaseDamage = new int[] { 1, 2, 3 },
				MaxHealth  = new int[] { 1, 2, 3 }
			});
			_id = _state.NewEntityId();
			_config.UnitLevels = new int[] { 100, 200 };
			var unit = new UnitState("unit", 1).WithId(_id);
			unit.Experience = 110;
			_state.AddUnit(unit);
		}

		[Fact]
		void CantLevelUpUnknownUnit() {
			IsInvalid(new LevelUpCommand(ulong.MaxValue));
		}

		[Fact]
		void CantLevelUpIfNoEnoughExperience() {
			_state.Units[_id].Experience = 99;

			IsInvalid(new LevelUpCommand(_id));
		}

		[Fact]
		void CantLevelUpIfNoMoreLevels() {
			_state.Units[_id].Experience = 300;
			Execute(new LevelUpCommand(_id));

			_state.Units[_id].Experience = int.MaxValue;
			IsInvalid(new LevelUpCommand(_id));
		}

		[Fact]
		void CantBeRunDirectly() {
			IsInvalidOnServer(new LevelUpCommand(_id));
		}

		[Fact]
		void IsLevelIncreased() {
			Execute(new LevelUpCommand(_id));

			Assert.Equal(1, _state.Units[_id].Level);
		}

		[Fact]
		void IsUnitHealed() {
			Execute(new LevelUpCommand(_id));

			Assert.Equal(2, _state.Units[_id].Health);
		}

		[Fact]
		void IsRestOfExperienceSaved() {
			Execute(new LevelUpCommand(_id));

			Assert.Equal(10, _state.Units[_id].Experience);
		}

		[Fact]
		void IsExperienceWastedOnLastLevel() {
			_state.Units[_id].Experience = 350;
			Execute(new LevelUpCommand(_id));

			Assert.Equal(0, _state.Units[_id].Experience);
		}
	}
}
