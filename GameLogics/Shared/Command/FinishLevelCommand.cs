using System;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Command {
	public sealed class FinishLevelCommand : IInternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		public bool IsValid(GameState state, ConfigRoot config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
				if ( !config.IsFeatureEnabled(Features.AutoHeal) ) {
					continue;
				}
				var unitConfig = config.Units[unit.Descriptor];
				if ( unit.Health < unitConfig.MaxHealth[unit.Level] ) {
					buffer.Add(new HealUnitCommand(unit.Id));
				}
			}

			var levelDesc = state.Level.Descriptor;
			var playerUnits = state.Level.PlayerUnits;

			state.Level = null;

			if ( !Win ) {
				return;
			}

			foreach ( var unit in playerUnits ) {
				if ( unit.Level >= config.UnitLevels.Length ) {
					continue;
				}
				var expAccum = 0;
				foreach ( var enemyDesc in config.Levels[levelDesc].EnemyDescriptors ) {
					var enemyConfig = config.Units[enemyDesc];
					expAccum += enemyConfig.Experience;
				}
				if ( expAccum > 0 ) {
					buffer.Add(new AddExperienceCommand(unit.Id, expAccum));
				}
			}

			var scope = LevelUtils.GetScope(levelDesc);
			state.Progress[scope] = Math.Min(state.Progress.GetOrDefault(scope) + 1, LevelUtils.GetIndex(levelDesc) + 1);
				
			var rewardLevel = config.Levels[levelDesc].RewardLevel;
			RewardLogic.AppendReward(rewardLevel, state, config, buffer);
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}