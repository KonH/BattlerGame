using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using Xunit;

namespace UnitTests {
	public class AttackCommandTest : BaseCommandTest<AttackCommand> {
		public AttackCommandTest() {
			_config.AddUnit("player_unit", new UnitConfig(1));
			_state.Level = new LevelState(
				"level_desc",
				new List<UnitState> { new UnitState("player_unit", 2).WithId("player_id") },
				new List<UnitState> { new UnitState("enemy_unit", 2).WithId("enemy_id") }
			);
		}
		
		[Fact]
		void CantAttackIfLevelNotStarted() {
			_state.Level = null;
			
			IsInvalid(new AttackCommand("player_id", "enemy_id"));
		}

		[Fact]
		void CantAttackByUnknownDealer() {
			IsInvalid(new AttackCommand("unknown_player_id", "enemy_id"));
		}

		[Fact]
		void CantAttackUnknownTarget() {
			IsInvalid(new AttackCommand("player_id", "unknown_enemy_id"));
		}
		
		[Fact]
		void CantAttackByDealerWithUnknownDescription() {
			_state.Level.PlayerUnits.Add(new UnitState("unknown_player_unit", 1).WithId("player_id_2"));
			
			IsInvalid(new AttackCommand("player_id_2", "enemy_id"));
		}
		
		[Fact]
		void IsDamageWasReceived() {
			Execute(new AttackCommand("player_id", "enemy_id"));
			
			Assert.Equal(1, _state.Level.EnemyUnits[0].Health);
		}
		
		[Fact]
		void IsUnitWithoutHpWillKilled() {
			_state.Level.EnemyUnits[0].Health = 1;
			
			Produces<KillUnitCommand>(new AttackCommand("player_id", "enemy_id"));
		}
	}
}