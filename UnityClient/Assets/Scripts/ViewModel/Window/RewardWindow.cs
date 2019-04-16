using System;
using System.Threading.Tasks;
using GameLogics.Shared.Command;
using UnityClient.Utils;
using UnityClient.Service;
using UnityClient.ViewModel.Fragment;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using TMPro;

namespace UnityClient.ViewModel.Window {
	public sealed class RewardWindow : BaseWindow {
		public sealed class Context {
			public string Header;
			public string ButtonText;
			public Action Callback;

			public Context(string header, string buttonText, Action callback) {
				Header     = header;
				ButtonText = buttonText;
				Callback   = callback;
			}
		}

		public sealed class Factory : PlaceholderFactory<Context, RewardWindow> {}

		public TMP_Text  Header;
		public TMP_Text  ButtonText;
		public Button    OkButton;
		public Transform ItemsRoot;

		ClientCommandRunner    _runner;
		RewardFragment.Factory _rewardFragment;

		Stack<RewardFragment> _nonPersistFragments = new Stack<RewardFragment>();

		[Inject]
		public void Init(ClientCommandRunner runner, RewardFragment.Factory rewardFragment, Canvas parent, Context context) {			
			_runner         = runner;
			_rewardFragment = rewardFragment;
			
			_runner.Updater.AddHandler<AddResourceCommand>  (OnAddResource);
			_runner.Updater.AddHandler<AddItemCommand>      (OnAddItem);
			_runner.Updater.AddHandler<AddUnitCommand>      (OnAddUnit);
			_runner.Updater.AddHandler<AddExperienceCommand>(OnAddExperience);
			_runner.Updater.AddHandler<LevelUpCommand>      (OnLevelUp);

			Header.text     = context.Header;
			ButtonText.text = context.ButtonText;
			OkButton.onClick.AddListener(() => Hide(context.Callback));
			
			ShowAt(parent);
		}

		void OnDestroy() {
			_runner.Updater.RemoveHandler<AddResourceCommand>  (OnAddResource);
			_runner.Updater.RemoveHandler<AddItemCommand>      (OnAddItem);
			_runner.Updater.RemoveHandler<AddUnitCommand>      (OnAddUnit);
			_runner.Updater.RemoveHandler<AddExperienceCommand>(OnAddExperience);
			_runner.Updater.RemoveHandler<LevelUpCommand>      (OnLevelUp);
		}

		async Task AddFragment(string text, bool persist = true) {
			if ( persist ) {
				if ( _nonPersistFragments.Count > 0 ) {
					await this.Wait(0.5f);
					while ( _nonPersistFragments.Count > 0 ) {
						var fragment = _nonPersistFragments.Pop();
						await fragment.Animation.PerformHide();
						fragment.gameObject.SetActive(false);
					}
				}
			}
			var instance = _rewardFragment.Create(ItemsRoot, text);
			await instance.Animation.PerformShow();
			if ( !persist ) {
				_nonPersistFragments.Push(instance);
			}
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

		Task OnAddExperience(AddExperienceCommand cmd) {
			return AddFragment($"{cmd.UnitId}: +{cmd.Amount} EXP", false);
		}

		Task OnLevelUp(LevelUpCommand cmd) {
			return AddFragment($"{cmd.UnitId}: +1 Level");
		}
	}
}
