using System;
using System.Threading.Tasks;
using GameLogics.Shared.Commands;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public sealed class WinWindow : BaseWindow {
		public sealed class Factory : PlaceholderFactory<Action, WinWindow> {}
		
		public Button    OkButton;
		public Transform ItemsRoot;

		ClientCommandRunner    _runner;
		RewardFragment.Factory _rewardFragment;

		[Inject]
		public void Init(ClientCommandRunner runner, RewardFragment.Factory rewardFragment, Canvas parent, Action callback) {			
			_runner         = runner;
			_rewardFragment = rewardFragment;
			
			_runner.Updater.AddHandler<AddResourceCommand>(OnAddResource);
			_runner.Updater.AddHandler<AddItemCommand>    (OnAddItem);
			_runner.Updater.AddHandler<AddUnitCommand>    (OnAddUnit);
			
			OkButton.onClick.AddListener(() => Hide(callback));
			
			ShowAt(parent);
		}

		void OnDestroy() {
			_runner.Updater.RemoveHandler<AddResourceCommand>(OnAddResource);
			_runner.Updater.RemoveHandler<AddItemCommand>    (OnAddItem);
			_runner.Updater.RemoveHandler<AddUnitCommand>    (OnAddUnit);
		}

		async Task AddFragment(string text) {
			var instance = _rewardFragment.Create(ItemsRoot, text);
			await instance.Animation.PerformShow();
		}
		
		Task OnAddResource(AddResourceCommand cmd) {
			return AddFragment($"+{cmd.Count} {cmd.Kind}");
		}

		Task OnAddItem(AddItemCommand cmd) {
			return AddFragment($"New item: {cmd.Id} ({cmd.Descriptor})");
		}

		Task OnAddUnit(AddUnitCommand cmd) {
			return AddFragment($"New unit: {cmd.Id} ({cmd.Descriptor})");
		}
	}
}
