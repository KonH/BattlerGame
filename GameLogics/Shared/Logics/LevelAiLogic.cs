using System.Collections.Generic;
using GameLogics.Shared.Command;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Logic {
	public static class LevelAiLogic {
		public static void AddCommands(GameState state, ConfigRoot config, ICommandBuffer buffer) {
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
				var damage = DamageLogic.GetDamage(state, config, enemyId, playerId);
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