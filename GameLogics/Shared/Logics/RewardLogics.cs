using System;
using System.Collections.Generic;
using System.Linq;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Logics {
	public static class RewardLogics {
		static List<Resource> _allResources = ((Resource[])Enum.GetValues(typeof(Resource))).Where(r => r != Resource.Unknown).ToList();
		
		public static Reward GenerateReward(string rewardLevel, Config config, Random random) {
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