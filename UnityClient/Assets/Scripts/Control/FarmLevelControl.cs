using GameLogics.Client.Service;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Utils;
using TMPro;
using UnityClient.Manager;
using UnityClient.ViewModel.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Control {
	public sealed class FarmLevelControl : MonoBehaviour {
		public Button   Button;
		public TMP_Text Text;

		ClientStateService       _service;
		GameSceneManager         _scene;
		StartLevelWindow.Factory _startWindow;

		float  _interval = 3.0f;
		float  _timer    = 0.0f;

		(string name, FarmConfig info) AvailableEvent {
			get {
				return FarmLogic.GetFirstAvailable(_service.State, _service.Config);
			}
		}

		string LevelDesc {
			get {
				var scope = AvailableEvent.name;
				return LevelUtils.GetDesc(scope, 0);
			}
		}

		[Inject]
		public void Init(ClientStateService service, GameSceneManager scene, StartLevelWindow.Factory startWindow) {
			_service     = service;
			_scene       = scene;
			_startWindow = startWindow;

			Button.onClick.AddListener(Execute);

			_timer = _interval;
		}

		public void Execute() {
			_startWindow.Create(LevelDesc, _scene.GoToLevel);
		}

		void Update() {
			if ( _timer < _interval ) {
				_timer += Time.deltaTime;
				return;
			}
			_timer = 0.0f;
			var ev = AvailableEvent;
			var exist = !string.IsNullOrEmpty(ev.name);
			Button.interactable = exist;
			Text.enabled = exist;
			if ( exist ) {
				Text.text = ev.name;
			}
		}
	}
}

