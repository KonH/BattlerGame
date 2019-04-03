using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLogics.Client.Services;
using GameLogics.Shared.Commands;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;
using TMPro;
using UnityClient.Models;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class UnitWindow : BaseWindow {		
		public class Factory : PlaceholderFactory<StateUnitModel, UnitWindow> {}
		
		public Button    CloseButton;
		public Transform ItemsRoot;

		public TMP_Text NameText;

		ClientCommandRunner          _runner;
		GameState                    _state;
		Config                       _config;

		ItemFragment.Factory _itemFragment;

		ItemsWindow.Factory _itemsWindow;
		
		StateUnitModel _unit;
		
		Dictionary<ItemType, ItemFragment> _fragments = new Dictionary<ItemType, ItemFragment>();

		ItemsWindow _equipWindow;

		[Inject]
		public void Init(
			ClientCommandRunner runner, ClientStateService service, ItemFragment.Factory itemFragment, ItemsWindow.Factory itemsWindow,
			Canvas parent, StateUnitModel unit
		) {			
			_runner       = runner;
			_state        = service.State;
			_config       = service.Config;
			_itemFragment = itemFragment;
			_itemsWindow = itemsWindow;
			_unit         = unit;

			_runner.Updater.AddHandler<EquipItemCommand>  (OnEquipItem);
			_runner.Updater.AddHandler<TakeOffItemCommand>(OnTakeOffItem);
			
			CloseButton.onClick.AddListener(() => Hide());
			
			NameText.text = unit.Name;
			CreateFragments();
			
			ShowAt(parent);
		}

		void OnDestroy() {
			_runner.Updater.RemoveHandler<EquipItemCommand>  (OnEquipItem);
			_runner.Updater.RemoveHandler<TakeOffItemCommand>(OnTakeOffItem);
		}

		Task OnEquipItem(EquipItemCommand cmd) {
			var state = _unit.State.Items.Find(it => it.Id == cmd.ItemId);
			var config = _config.Items[state.Descriptor];
			ReplaceFragment(config.Type, CreateItemViewModel(state, config));

			if ( _equipWindow ) {
				_equipWindow.Hide();
				_equipWindow = null;
			}
			
			return Task.CompletedTask;
		}

		Task OnTakeOffItem(TakeOffItemCommand cmd) {
			var state  = _state.Items[cmd.ItemId];
			var config = _config.Items[state.Descriptor];
			ReplaceFragment(config.Type, CreateItemPlaceholder(config.Type));
			return Task.CompletedTask;
		}
		
		void CreateFragments() {
			foreach ( var state in _unit.State.Items ) {
				var itemConfig = _config.Items[state.Descriptor];
				AddFragment(CreateItemViewModel(state, itemConfig));
			}
			foreach ( ItemType type in Enum.GetValues(typeof(ItemType)) ) {
				if ( type == ItemType.Unknown ) {
					continue;
				}
				if ( _fragments.ContainsKey(type) ) {
					continue;
				}
				AddFragment(CreateItemPlaceholder(type));
			}
		}

		ItemModel CreateItemViewModel(ItemState state, ItemConfig config) {
			return new StateItemModel(
				state, config,
				new ClickAction<ItemModel>("Take Off", it => {
					var item = (StateItemModel)it;
					_runner.TryAddCommand(new TakeOffItemCommand(item.State.Id, _unit.State.Id));
				})
			);
		}

		ItemModel CreateItemPlaceholder(ItemType type) {
			return new PlaceholderItemModel(
				type,
				new ClickAction<ItemModel>("Equip", it => {
					var items = new List<ItemModel>();
					var wantedItems = _state.Items.Values.Where(i => _config.Items[i.Descriptor].Type == type).ToList();
					foreach ( var state in wantedItems ) {
						items.Add(new StateItemModel(state, _config.Items[state.Descriptor], new ClickAction<ItemModel>("Equip", item => {
							_runner.TryAddCommand(new EquipItemCommand(state.Id, _unit.State.Id));
						})));
					}
					_equipWindow = _itemsWindow.Create(items);
				})
			);
		}

		void RemoveFragment(ItemType type) {
			_fragments[type].gameObject.SetActive(false);
			_fragments.Remove(type);
		}
		
		void AddFragment(ItemModel model) {
			var fragment = _itemFragment.Create(ItemsRoot, model);
			_fragments.Add(model.Type, fragment);
		}
		
		void ReplaceFragment(ItemType type, ItemModel model) {
			RemoveFragment(type);
			AddFragment(model);
		}
	}
}
