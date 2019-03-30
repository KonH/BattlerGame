using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using TMPro;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;

namespace UnityClient.ViewModels.Windows {
	public class UnitWindow : BaseWindow {
		public Button    CloseButton;
		public Transform ItemsRoot;

		public TMP_Text NameText;

		public BaseAnimation Animation;

		ClientCommandRunner          _runner;
		GameState                    _state;
		Config                       _config;
		Action<UnitModel, ItemModel> _onEquipItem;
		
		UnitModel    _unit;
		ItemFragment _itemTemplate;
		
		Dictionary<ItemType, ItemFragment> _items = new Dictionary<ItemType, ItemFragment>();
		
		void Awake() {
			Animation.Show();
		}

		public void Show(ClientCommandRunner runner, GameState state, Config config, Action<UnitModel, ItemModel> onEquipItem, UnitModel unit, ItemFragment itemTemplate) {
			_runner       = runner;
			_state        = state;
			_config       = config;
			_onEquipItem  = onEquipItem;
			_unit         = unit;
			_itemTemplate = itemTemplate;

			_runner.Updater.AddHandler<EquipItemCommand>  (OnEquipItem);
			_runner.Updater.AddHandler<TakeOffItemCommand>(OnTakeOffItem);
			
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			NameText.text = unit.State.Id.ToString();
			
			itemTemplate.transform.SetParent(ItemsRoot, false);
			itemTemplate.gameObject.SetActive(false);
			var items = CollectItems();
			foreach ( var item in items ) {
				AddItem(item);
			}
		}

		void OnDestroy() {
			_runner.Updater.RemoveHandler<EquipItemCommand>  (OnEquipItem);
			_runner.Updater.RemoveHandler<TakeOffItemCommand>(OnTakeOffItem);
		}

		Task OnEquipItem(EquipItemCommand cmd) {
			var state = _unit.State.Items.Find(it => it.Id == cmd.ItemId);
			var config = _config.Items[state.Descriptor];
			RemoveItem(config.Type);
			AddItem(new ItemModel(state, config));
			return Task.CompletedTask;
		}

		Task OnTakeOffItem(TakeOffItemCommand cmd) {
			var state  = _state.Items[cmd.ItemId];
			var config = _config.Items[state.Descriptor];
			RemoveItem(config.Type);
			AddItem(new ItemModel(config.Type));
			return Task.CompletedTask;
		}

		void RemoveItem(ItemType type) {
			_items[type].gameObject.SetActive(false);
			_items.Remove(type);
		}
		
		void AddItem(ItemModel item) {
			var instance = Instantiate(_itemTemplate, ItemsRoot, false);
			instance.gameObject.SetActive(true);
			var actName  = item.IsFake ? "Equip" : "Take off";
			instance.Init(item, actName, OnClickItem);
			_items.Add(item.Config?.Type ?? item.FakeType, instance);
		}
		
		List<ItemModel> CollectItems() {
			var items = new List<ItemModel>();
			foreach ( var state in _unit.State.Items ) {
				var itemConfig = _config.Items[state.Descriptor];
				items.Add(new ItemModel(state, itemConfig));
			}
			foreach ( ItemType type in Enum.GetValues(typeof(ItemType)) ) {
				if ( type == ItemType.Unknown ) {
					continue;
				}
				if ( items.Find(it => !it.IsFake && (it.Config.Type == type)) != null ) {
					continue;
				}
				items.Add(new ItemModel(type));
			}
			return items;
		}

		void OnClickItem(ItemModel item) {
			if ( item.IsFake ) {
				_onEquipItem(_unit, item);
			} else {
				_runner.TryAddCommand(new TakeOffItemCommand(item.State.Id, _unit.State.Id));
			}
		}
	}
}
