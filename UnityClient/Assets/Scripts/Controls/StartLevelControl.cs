using UnityClient.Managers;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.Controls {
	public class StartLevelControl : MonoBehaviour {
		public string LevelDesc;

		public Button Button;

		StartLevelWindow.Factory _startLevelWindow;
		GameSceneManager         _scene;
		
		[Inject]
		public void Init(StartLevelWindow.Factory startLevelWindow, GameSceneManager scene) {
			_startLevelWindow = startLevelWindow;
			_scene            = scene;
			
			Button.onClick.AddListener(Execute);
		}

		public void Execute() {
			_startLevelWindow.Create(LevelDesc, _scene.GoToLevel);
		}
	}
}

