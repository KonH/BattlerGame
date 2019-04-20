using GameLogics.Client.Service;
using GameLogics.Shared.Command;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;
using TMPro;
using UnityClient.Manager;
using UnityClient.Service;
using UnityClient.ViewModel.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Control {
	public sealed class EventLevelControl : MonoBehaviour {
		public Button   Button;
		public TMP_Text Text;

		ClientStateService       _service;
		ClientCommandRunner      _runner;
		GameSceneManager         _scene;
		RewardWindow.Factory     _rewardWindow;
		StartLevelWindow.Factory _startWindow;

		float  _interval = 3.0f;
		float  _timer    = 0.0f;

		(string name, EventConfig info) ActiveEvent {
			get {
				return EventLogic.GetFirstActive(_service.State, _service.Config);
			}
		}

		string NextLevelDesc {
			get {
				var scope = ActiveEvent.info.Scope;
				var progress = _service.State.Progress.GetOrDefault(scope);
				return LevelUtils.GetDesc(scope, progress);
			}
		}

		bool IsCompleted {
			get {
				return _runner.IsValid(new ClaimEventRewardCommand(ActiveEvent.name));
			}
		}

		[Inject]
		public void Init(
			ClientStateService service, ClientCommandRunner runner, GameSceneManager scene,
			StartLevelWindow.Factory startWindow, RewardWindow.Factory rewardWindow
		) {
			_service      = service;
			_runner       = runner;
			_scene        = scene;
			_startWindow  = startWindow;
			_rewardWindow = rewardWindow;

			Button.onClick.AddListener(Execute);

			_timer = _interval;
		}

		public void Execute() {
			if ( IsCompleted ) {
				if ( _runner.TryAddCommand(new ClaimEventRewardCommand(ActiveEvent.name)) ) {
					var ctx = new RewardWindow.Context("Event completed!", "Claim", null);
					_rewardWindow.Create(ctx);
				}
			} else {
				_startWindow.Create(NextLevelDesc, _scene.GoToLevel);
			}
		}

		void Update() {
			if ( _timer < _interval ) {
				_timer += Time.deltaTime;
				return;
			}
			_timer = 0.0f;
			var ev = ActiveEvent;
			var exist = !string.IsNullOrEmpty(ev.name);
			Button.interactable = exist;
			Text.enabled = exist;
			if ( exist ) {
				Text.text = ev.name;
			}
		}
	}
}

