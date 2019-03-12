using GameLogics.Managers;
using GameLogics.Managers.Auth;
using GameLogics.Models;
using TMPro;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class LoginControl : MonoBehaviour {
		public TMP_InputField LoginInput;
		public TMP_InputField PasswordInput;

		MainThreadRunner _runner;
		GameSceneManager _sceneManager;
		IAuthManager     _authManager;
		UserManager      _userManager;

		[Inject]
		public void Init(MainThreadRunner runner, GameSceneManager sceneManager, IAuthManager authManager, UserManager userManager) {
			_runner       = runner;
			_sceneManager = sceneManager;
			_authManager  = authManager;
			_userManager  = userManager;

			LoginInput.text    = "test";
			PasswordInput.text = "test";
		}

		public void Login() {
			_runner.Run(async () => {
				var login    = LoginInput.text;
				var password = PasswordInput.text;
				var user     = User.CreateWithPassword(login, password, login, "user");
				_userManager.CurrentUser = user;
				var success  = await _authManager.TryLogin();
				if ( success ) {
					_sceneManager.GoToWorld();
				}
			});
		}

		public void Register() {
			_sceneManager.GoToRegister();
		}
	}
}
