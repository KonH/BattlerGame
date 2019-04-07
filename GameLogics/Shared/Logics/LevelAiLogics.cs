using System.Collections.Generic;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Logics {
	public static class LevelAiLogics {
		public static void AddCommands(GameState state, Config config, ICommandBuffer buffer) {
			var level = state.Level;
			var damages = new Dictionary<ulong, int>();
			foreach ( var enemy in level.EnemyUnits ) {
				var enemyId = enemy.Id;
				if ( !TrySelectPlayerToAttack(level.PlayerUnits, damages, out var playerUnit) ) {
					break;
				}
				var playerId = playerUnit.Id;
				var attackCommand = new AttackCommand(enemyId, playerId);
				buffer.Add(attackCommand);
				var damage = DamageLogics.GetDamage(state, config, enemyId, playerId);
				damages[playerId] = damages.GetOrDefault(playerId) + damage;
			}
			buffer.Add(new EndEnemyTurnCommand());
		}

		static bool TrySelectPlayerToAttack(List<UnitState> playerUnits, Dictionary<ulong, int> damages, out UnitState playerUnit) {
			foreach ( var unit in playerUnits ) {
				var curDamage = damages.GetOrDefault(unit.Id);
				if ( curDamage < unit.Health ) {
					playerUnit = unit;
					return true;
				}
			}
			playerUnit = null;
			return false;
		}
	}
}