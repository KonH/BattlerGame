using System;
using System.Collections.Generic;
using System.Linq;
using GameLogics.Shared.Command;
using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Logic {
	public static class RewardLogic {
		static List<Resource> _allResources = ((Resource[])Enum.GetValues(typeof(Resource))).Where(r => r != Resource.Unknown).ToList();

		public static void AppendReward(string rewardLevel, GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var reward = RewardLogic.GenerateReward(rewardLevel, config, state.CreateRandom());
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

		static Reward GenerateReward(string rewardLevel, ConfigRoot config, Random random) {
			var reward = new Reward();
			var rewardConfigs = config.Rewards[rewardLevel];
			var rewardConfig = rewardConfigs[random.Next(rewardConfigs.Count)];
			AddResources(reward, rewardConfig.Resources, random);
			AddItems(reward, rewardConfig.Items, config.Items, random);
			AddUnits(reward, rewardConfig.Units, config.Units, random);
			return reward;
		}

		static void AddResources(Reward reward, RewardInterval interval, Random random) {
			if ( interval.Max == 0 ) {
				return;
			}
			var resource = _allResources[random.Next(_allResources.Count)];
			reward.Resources.Add(resource, random.Next(interval.Min, interval.Max + 1));
		}
		
		static void AddItems(Reward reward, RewardInterval interval, Dictionary<string, BaseItemConfig> items, Random random) {
			AddByDesc(reward.Items, interval, items.Keys.ToList(), random);
		}
		
		static void AddUnits(Reward reward, RewardInterval interval, Dictionary<string, UnitConfig> units, Random random) {
			AddByDesc(reward.Units, interval, units.Keys.ToList(), random);
		}

		static void AddByDesc(List<string> reward, RewardInterval interval, List<string> descs, Random random) {
			var count = random.Next(interval.Min, interval.Max + 1);
			for ( var i = 0; i < count; i++ ) {
				var itemDesc = descs[random.Next(descs.Count)];
				reward.Add(itemDesc);
			}
		}
	}
}