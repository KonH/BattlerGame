using System;
using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Logics;
using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Commands {
	public sealed class FinishLevelCommand : IInternalCommand {
		public readonly bool Win;

		public FinishLevelCommand(bool win) {
			Win = win;
		}
		
		public bool IsValid(GameState state, Config config) {
			if ( state.Level == null ) {
				return false;
			}
			return true;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			foreach ( var unit in state.Level.PlayerUnits ) {
				state.AddUnit(unit);
				if ( !config.IsFeatureEnabled(Features.AutoHeal) ) {
					continue;
				}
				var unitConfig = config.Units[unit.Descriptor];
				if ( unit.Health < unitConfig.MaxHealth ) {
					buffer.Add(new HealUnitCommand(unit.Id));
				}
			}

			var levelDesc = state.Level.Descriptor;

			state.Level = null;

			if ( !Win ) {
				return;
			}

			var scope = LevelUtils.GetScope(levelDesc);
			state.Progress[scope] = Math.Min(state.Progress.GetOrDefault(scope) + 1, LevelUtils.GetIndex(levelDesc) + 1);
				
			var rewardLevel = config.Levels[levelDesc].RewardLevel;
			var reward = RewardLogics.GenerateReward(rewardLevel, config, state.CreateRandom());
			foreach ( var pair in reward.Resources ) {
				buffer.Add(new AddResourceCommand(pair.Key, pair.Value));
			}
			foreach ( var itemDesc in reward.Items ) {
				buffer.Add(new AddItemCommand(state.NewEntityId(), itemDesc));
			}
			foreach ( var unitDesc in reward.Units ) {
				buffer.Add(new AddUnitCommand(state.NewEntityId(), unitDesc));
			}
			buffer.Add(new UpdateRandomSeedCommand());
		}

		public override string ToString() {
			return $"{nameof(FinishLevelCommand)} ({Win})";
		}
	}
}