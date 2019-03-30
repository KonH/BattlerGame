using System;
using System.Threading.Tasks;
using GameLogics.Shared.Commands;
using UnityClient.Services;
using UnityClient.ViewModels.Fragments;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace UnityClient.ViewModels.Windows {
	public class WinWindow : BaseWindow {
		public Button    OkButton;
		public Transform ItemsRoot;

		public BaseAnimation Animation;

		ClientCommandRunner _runner;
		RewardFragment      _rewardTemplate;
		
		void Awake() {
			Animation.Show();
		}

		public void Show(ClientCommandRunner runner, RewardFragment rewardTemplate, Action callback) {			
			_runner         = runner;
			_rewardTemplate = rewardTemplate;
			
			OkButton.onClick.AddListener(() => Animation.Hide(callback));
			
			_rewardTemplate.transform.SetParent(ItemsRoot);
			_rewardTemplate.gameObject.SetActive(false);
			
			_runner.Updater.AddHandler<AddResourceCommand>(OnAddResource);
			_runner.Updater.AddHandler<AddItemCommand>    (OnAddItem);
			_runner.Updater.AddHandler<AddUnitCommand>    (OnAddUnit);
		}

		void OnDestroy() {
			_runner.Updater.RemoveHandler<AddResourceCommand>(OnAddResource);
			_runner.Updater.RemoveHandler<AddItemCommand>    (OnAddItem);
			_runner.Updater.RemoveHandler<AddUnitCommand>    (OnAddUnit);
		}

		async Task AddFragment(string text) {
			var instance = Instantiate(_rewardTemplate, ItemsRoot, false);
			instance.gameObject.SetActive(true);
			instance.Init(text);
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
