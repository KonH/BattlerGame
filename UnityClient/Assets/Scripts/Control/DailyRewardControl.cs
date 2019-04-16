using GameLogics.Shared.Command;
using UnityClient.Service;
using UnityClient.ViewModel.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Control {
	public sealed class DailyRewardControl : MonoBehaviour {
		public Button Button;

		ClientCommandRunner  _runner;
		RewardWindow.Factory _rewardWindow;

		ClaimDailyRewardCommand _command  = new ClaimDailyRewardCommand();
		float                   _interval = 3.0f;
		float                   _timer    = 0.0f;

		[Inject]
		public void Init(ClientCommandRunner runner, RewardWindow.Factory rewardWindow) {
			_runner       = runner;
			_rewardWindow = rewardWindow;

			Button.onClick.AddListener(Execute);
		}

		public void Execute() {
			if ( _runner.TryAddCommand(_command) ) {
				var ctx = new RewardWindow.Context("Daily Reward!", "Claim", null);
				_rewardWindow.Create(ctx);
			}
		}

		void Update() {
			if ( _timer < _interval ) {
				_timer += Time.deltaTime;
				return;
			}
			_timer = 0.0f;
			Button.interactable = _runner.IsValid(_command);
		}
	}
}

