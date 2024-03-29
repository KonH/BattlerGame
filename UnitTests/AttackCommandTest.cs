using System.Collections.Generic;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using Xunit;

namespace UnitTests {
	public sealed class AttackCommandTest : BaseCommandTest<AttackCommand> {
		ulong _playerId;
		ulong _enemyId;
		
		public AttackCommandTest() {
			_config
				.AddUnit("player_unit", new UnitConfig(1, 1))
				.AddLevel("level_0", new LevelConfig { RewardLevel = "" })
				.AddReward("", new RewardConfig());
			_playerId = NewId();
			_enemyId = NewId();
			_state.Level = new LevelState(
				"level_0",
				new List<UnitState> { new UnitState("player_unit", 2).WithId(_playerId) },
				new List<UnitState> { new UnitState("enemy_unit", 2).WithId(_enemyId) }
			);
			_state.Level.PlayerTurn = true;
		}
		
		[Fact]
		void CantAttackIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new AttackCommand(_playerId, _enemyId));
		}

		[Fact]
		void CantAttackByUnknownDealer() {
			IsInvalid(new AttackCommand(InvalidId, _enemyId));
		}

		[Fact]
		void CantAttackUnknownTarget() {
			IsInvalid(new AttackCommand(_playerId, InvalidId));
		}
		
		[Fact]
		void CantAttackByDealerWithUnknownDescription() {
			var id = NewId();
			_state.Level.PlayerUnits.Add(new UnitState("unknown_player_unit", 1).WithId(id));
			
			IsInvalid(new AttackCommand(id, _enemyId));
		}
		
		[Fact]
		void IsDamageWasReceived() {
			Execute(new AttackCommand(_playerId, _enemyId));
			
			Assert.Equal(1, _state.Level.EnemyUnits[0].Health);
		}
		
		[Fact]
		void IsUnitWithoutHpWillKilled() {
			_state.Level.EnemyUnits[0].Health = 1;
			
			Produces<KillUnitCommand>(new AttackCommand(_playerId, _enemyId));
		}

		[Fact]
		void CantAttackIfOtherSideTurn() {
			_state.Level.PlayerTurn = false;
			
			IsInvalid(new AttackCommand(_playerId, _enemyId));
		}

		[Fact]
		void IsUnitAddedToMovedUnits() {
			Execute(new AttackCommand(_playerId, _enemyId));

			Assert.Contains(_state.Level.MovedUnits, id => (id == _playerId));
		}
		
		[Fact]
		void CantAttackAlreadyMovedUnit() {
			_state.Level.MovedUnits.Add(_playerId);
			
			IsInvalid(new AttackCommand(_playerId, _enemyId));
		}

		[Fact]
		void AttackWithWeaponIncreaseDamage() {
			_state.Level.EnemyUnits[0].Health = 3;
			_config.AddItem("weapon", new WeaponConfig(1));
			_state.Level.PlayerUnits[0].Items.Add(new ItemState("weapon"));

			Execute(new AttackCommand(_playerId, _enemyId));
			
			Assert.Equal(1, _state.Level.EnemyUnits[0].Health);
		}
		
		[Fact]
		void AttackToArmorDecreaseDamage() {
			_config.AddItem("armor", new ArmorConfig(1));
			_state.Level.EnemyUnits[0].Items.Add(new ItemState("armor"));

			Execute(new AttackCommand(_playerId, _enemyId));
			
			Assert.Equal(2, _state.Level.EnemyUnits[0].Health);
		}
		
		[Fact]
		void AttackToArmorDontHeal() {
			_config.AddItem("armor", new ArmorConfig(2));
			_state.Level.EnemyUnits[0].Items.Add(new ItemState("armor"));

			Execute(new AttackCommand(_playerId, _enemyId));
			
			Assert.Equal(2, _state.Level.EnemyUnits[0].Health);
		}
	}
}