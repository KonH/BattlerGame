using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using TMPro;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public sealed class UnitWindow : BaseWindow {		
		public sealed class Factory : PlaceholderFactory<StateUnitModel, UnitWindow> {}
		
		public Button    CloseButton;
		public Transform ItemsRoot;
		public TMP_Text  NameText;

		GameStateUpdateService _update;
		ItemService            _items;
		ItemFragment.Factory   _itemFragment;
		ItemsWindow.Factory    _itemsWindow;
		StateUnitModel         _unit;

		Dictionary<ItemType, ItemFragment> _fragments   = new Dictionary<ItemType, ItemFragment>();
		ItemsWindow                        _equipWindow = null;

		[Inject]
		public void Init(
			GameStateUpdateService update, ItemService items, ItemFragment.Factory itemFragment, ItemsWindow.Factory itemsWindow,
			Canvas parent, StateUnitModel unit
		) {
			_update       = update;
			_items        = items;
			_itemFragment = itemFragment;
			_itemsWindow  = itemsWindow;
			_unit         = unit;

			update.AddHandler<EquipItemCommand>  (OnEquipItem);
			update.AddHandler<TakeOffItemCommand>(OnTakeOffItem);
			
			CloseButton.onClick.AddListener(Hide);
			
			NameText.text = unit.Name;
			CreateFragments();
			
			ShowAt(parent);
		}

		void OnDestroy() {
			_update.RemoveHandler<EquipItemCommand>  (OnEquipItem);
			_update.RemoveHandler<TakeOffItemCommand>(OnTakeOffItem);
		}

		Task OnEquipItem(EquipItemCommand cmd) {
			var model = _items.CreateModel(_unit, cmd.ItemId, TakeOffItem);
			ReplaceFragment(model);
			TryHideEquipWindow();
			return Task.CompletedTask;
		}

		Task OnTakeOffItem(TakeOffItemCommand cmd) {
			var model = _items.CreatePlaceholder(cmd.ItemId, OpenEquipWindow);
			ReplaceFragment(model);
			return Task.CompletedTask;
		}
		
		ClickAction<ItemModel> TakeOffItem => new ClickAction<ItemModel>(
			"Take Off",
			it => _items.TakeOff(_unit, it)
		);
		
		ClickAction<ItemModel> OpenEquipWindow => new ClickAction<ItemModel>(
			"Equip",
			it => {
				var items = _items.GetItemsForEquip(it.Type, EquipItem);
				_equipWindow = _itemsWindow.Create(items);
			});
		
		ClickAction<ItemModel> EquipItem => new ClickAction<ItemModel>(
			"Equip",
			it => _items.Equip(_unit, it)
		);
		
		void CreateFragments() {
			var unitItems = _items.GetAllUnitItems(_unit, onItem: TakeOffItem, onPlaceholder: OpenEquipWindow);
			foreach ( var item in unitItems ) {
				AddFragment(item);
			}
		}

		void RemoveFragment(ItemType type) {
			_fragments[type].gameObject.SetActive(false);
			_fragments.Remove(type);
		}
		
		void AddFragment(ItemModel model) {
			var fragment = _itemFragment.Create(ItemsRoot, model);
			_fragments.Add(model.Type, fragment);
		}
		
		void ReplaceFragment(ItemModel model) {
			RemoveFragment(model.Type);
			AddFragment(model);
		}

		void TryHideEquipWindow() {
			if ( _equipWindow ) {
				_equipWindow.Hide();
				_equipWindow = null;
			}
		}
	}
}
