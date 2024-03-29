using System;
using System.Collections.Generic;
using GameLogics.Client.Service;
using GameLogics.Shared.Command;
using GameLogics.Shared.Model;
using GameLogics.Shared.Model.State;
using GameLogics.Shared.Model.Config;
using UnityClient.Model;
using System.Linq;

namespace UnityClient.Service {
	public sealed class ItemService {
		readonly ClientStateService  _stateService;
		readonly ClientCommandRunner _runner;
		
		ItemType[]        _allItemTypes = null;
		HashSet<ItemType> _usedTypes    = new HashSet<ItemType>();

		GameState  State  => _stateService.State;
		ConfigRoot Config => _stateService.Config;
		
		public ItemService(ClientStateService stateService, ClientCommandRunner runner) {
			_stateService = stateService;
			_runner       = runner;
			
			_allItemTypes = Enum.GetValues(typeof(ItemType)) as ItemType[];
		}

		public ItemModel CreateModel(ItemState state, ClickAction<ItemModel> onClick) {
			var config = GetItemConfig(state);
			return new StateItemModel(state, config, onClick);
		}
		
		public ItemModel CreateModel(StateUnitModel unit, ulong itemId, ClickAction<ItemModel> onClick) {
			var state = GetItemState(unit, itemId);
			return CreateModel(state, onClick);
		}

		public ItemModel CreatePlaceholder(ulong itemId, ClickAction<ItemModel> onClick) {
			var config = GetItemConfig(GetItemState(itemId));
			return new PlaceholderItemModel(config.Type, onClick);
		}
		
		public ItemModel CreatePlaceholder(ItemType type, ClickAction<ItemModel> onClick) {
			return new PlaceholderItemModel(type, onClick);
		}

		public List<ItemModel> GetAllUnitItems(StateUnitModel unit, ClickAction<ItemModel> onItem, ClickAction<ItemModel> onPlaceholder) {
			var result = new List<ItemModel>();
			FillUnitItems(unit, onItem, result);
			FillUnitPlaceholders(unit, onPlaceholder, result);
			return result;
		}

		public List<ItemModel> GetItemsForEquip(ItemType type, ClickAction<ItemModel> onClick) {
			var result = new List<ItemModel>();
			foreach ( var state in State.Items.Values ) {
				var config = GetItemConfig(state);
				if ( config.Type != type ) {
					continue;
				}
				result.Add(CreateModel(state, onClick));
			}
			return result;
		}

		public void Equip(StateUnitModel unit, ItemModel item) {
			var stateItem = (StateItemModel)item;
			_runner.TryAddCommand(new EquipItemCommand(stateItem.State.Id, unit.State.Id));
		}

		public void TakeOff(StateUnitModel unit, ItemModel item) {
			var stateItem = (StateItemModel)item;
			_runner.TryAddCommand(new TakeOffItemCommand(stateItem.State.Id, unit.State.Id));
		}
		
		void FillUnitItems(StateUnitModel unit, ClickAction<ItemModel> onClick, List<ItemModel> result) {
			foreach ( var state in GetItemStates(unit) ) {
				result.Add(CreateModel(state, onClick));
			}
		}

		void FillUnitPlaceholders(StateUnitModel unit, ClickAction<ItemModel> onClick, List<ItemModel> result) {
			var usedTypes = GetUsedTypes(unit);
			foreach ( var type in _allItemTypes ) {
				if ( usedTypes.Contains(type) ) {
					continue;
				}
				result.Add(CreatePlaceholder(type, onClick));
			}
		}
		
		HashSet<ItemType> GetUsedTypes(StateUnitModel unit) {
			_usedTypes.Clear();
			var result = _usedTypes;
			foreach ( var state in GetItemStates(unit) ) {
				var config = GetItemConfig(state);
				result.Add(config.Type);
			}
			return result;
		}

		public bool HasUpgrade(ItemState item) {
			var level = item.Level;
			var upgradeLevels = GetItemConfig(item).UpgradePrice;
			return (level < upgradeLevels.Length);
		}

		public string GetUpgradePriceStr(ItemState item) {
			if ( !HasUpgrade(item) ) {
				return string.Empty;
			}
			var level = item.Level;
			var upgradeLevels = GetItemConfig(item).UpgradePrice;
			var nextUpgradeLevel = upgradeLevels[level];
			return string.Join(",", nextUpgradeLevel.Select(p => $"{p.Key}: {p.Value}"));
		}

		public void Upgrade(ulong itemId) => _runner.TryAddCommand(new UpgradeItemCommand(itemId));

		public bool CanUpgrade(ulong itemId) => _runner.IsValid(new UpgradeItemCommand(itemId));

		List<ItemState> GetItemStates(StateUnitModel unit) => unit.State.Items;

		BaseItemConfig GetItemConfig(ItemState itemState) => Config.Items[itemState.Descriptor];

		ItemState GetItemState(StateUnitModel unit, ulong itemId) => unit.State.Items.Find(it => it.Id == itemId);

		ItemState GetItemState(ulong itemId) => State.Items[itemId];
	}
}