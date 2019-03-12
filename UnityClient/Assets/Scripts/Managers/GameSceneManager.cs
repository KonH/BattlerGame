﻿using UnityEngine.SceneManagement;

namespace UnityClient.Managers {
	public class GameSceneManager {
		const string StartScene    = "00_Start";
		const string LoginScene    = "01_Login";
		const string RegisterScene = "02_Register";
		const string WorldScene    = "03_World";
		
		string ActiveScene => SceneManager.GetActiveScene().name;
		
		public void GoToStart()    => OpenScene(StartScene);
		public void GoToLogin()    => OpenScene(LoginScene);
		public void GoToRegister() => OpenScene(RegisterScene);
		public void GoToWorld()    => OpenScene(WorldScene);

		void OpenScene(string name) {
			if ( ActiveScene == name ) {
				return;
			}
			SceneManager.LoadScene(name);
		}
	}
}