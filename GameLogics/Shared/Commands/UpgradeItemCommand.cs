using GameLogics.Shared.Commands.Base;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using GameLogics.Shared.Models.State;
using GameLogics.Shared.Utils;
using System.Collections.Generic;

namespace GameLogics.Shared.Commands {
	public sealed class UpgradeItemCommand : ICommand {
		public readonly ulong Id;

		public UpgradeItemCommand(ulong id) {
			Id = id;
		}

		public bool IsValid(GameState state, Config config) {
			if ( !state.Items.TryGetValue(Id, out var item) ) {
				return false;
			}
			var upgradePrices = GetUpgradePrices(config, item.Descriptor);
			if ( item.Level >= upgradePrices.Length ) {
				return false;
			}
			foreach ( var pair in upgradePrices[item.Level] ) {
				var res = pair.Key;
				var price = pair.Value;
				if ( state.Resources.GetOrDefault(res) < price ) {
					return false;
				} 
			}
			return true;
		}

		public void Execute(GameState state, Config config, ICommandBuffer buffer) {
			var item = state.Items[Id];
			var upgradePrices = GetUpgradePrices(config, item.Descriptor)[item.Level];
			foreach ( var pair in upgradePrices ) {
				var res   = pair.Key;
				var price = pair.Value;
				buffer.Add(new SpendResourceCommand(res, price));
			}
			item.Level++;
		}

		Dictionary<Resource, int>[] GetUpgradePrices(Config config, string desc) => config.Items[desc].UpgradePrice;

		public override string ToString() {
			return $"{nameof(UpgradeItemCommand)} ({Id})";
		}
	}
}
