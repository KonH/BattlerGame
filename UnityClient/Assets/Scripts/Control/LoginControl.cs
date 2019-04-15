using GameLogics.Client.Model;
using GameLogics.Client.Service;
using TMPro;
using UnityClient.Manager;
using UnityClient.Service;
using UnityEngine;
using Zenject;

namespace UnityClient.Control {
	public sealed class LoginControl : MonoBehaviour {
		public TMP_InputField LoginInput;
		public TMP_InputField PasswordInput;

		MainThreadRunner _runner;
		GameSceneManager _scene;
		AuthService      _auth;

		[Inject]
		public void Init(MainThreadRunner runner, GameSceneManager sceneManager, AuthService auth) {
			_runner = runner;
			_scene  = sceneManager;
			_auth   = auth;

			LoginInput.text    = "test";
			PasswordInput.text = "test";
		}

		public void Login() {
			_runner.Run(async () => {
				var login    = LoginInput.text;
				var password = PasswordInput.text;
				var user     = new UserState(login, password, login);
				var success  = await _auth.TryLogin(user);
				if ( success ) {
					_scene.GoToWorld();
				}
			});
		}

		public void Register() {
			_scene.GoToRegister();
		}
	}
}
