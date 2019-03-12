using GameLogics.Managers;
using GameLogics.Models;
using TMPro;
using UnityClient.Managers;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class RegisterControl : MonoBehaviour {
		public TMP_InputField Login;
		public TMP_InputField Password;

		MainThreadRunner _runner;
		GameSceneManager _sceneManager;
		RegisterManager  _registerManager;
		UserManager      _userManager;

		[Inject]
		public void Init(MainThreadRunner runner, GameSceneManager sceneManager, RegisterManager registerManager, UserManager userManager) {
			_runner          = runner;
			_sceneManager    = sceneManager;
			_registerManager = registerManager;
			_userManager     = userManager;
		}
		
		public void Register() {
			_runner.Run(async () => {
				var login    = Login.text;
				var password = Password.text;
				var user     = User.CreateWithPassword(login, password, login, "user");
				_userManager.CurrentUser = user;
				var success  = await _registerManager.TryRegister();
				if ( success ) {
					_sceneManager.GoToLogin();
				}
			});
		}
	}
}
