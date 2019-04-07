using GameLogics.Client.Services;
using GameLogics.Shared.Utils;
using TMPro;
using UnityClient.Managers;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public sealed class StartLevelControl : MonoBehaviour {
		public TMP_Text Text;
		public Button   Button;

		ClientStateService       _state;
		StartLevelWindow.Factory _startLevelWindow;
		GameSceneManager         _scene;
		
		[Inject]
		public void Init(ClientStateService state, StartLevelWindow.Factory startLevelWindow, GameSceneManager scene) {
			_state            = state;
			_startLevelWindow = startLevelWindow;
			_scene            = scene;

			if ( HasLevel() ) {
				Text.text = GetLevelDesc();
				Button.onClick.AddListener(Execute);
			} else {
				Text.text = "All done!";
				Button.interactable = false;
			}
		}

		int GetLevelIndex() {
			return _state.State.Progress.GetOrDefault("level");
		}

		string GetLevelDesc() {
			return "level_" + GetLevelIndex();
		}

		bool HasLevel() {
			return _state.Config.Levels.ContainsKey(GetLevelDesc());
		}

		public void Execute() {
			_startLevelWindow.Create(GetLevelDesc(), _scene.GoToLevel);
		}
	}
}

