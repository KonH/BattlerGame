using GameLogics.Client.Model;
using GameLogics.Client.Service;
using TMPro;
using UnityClient.Manager;
using UnityClient.Service;
using UnityEngine;
using Zenject;

namespace UnityClient.Control {
	public sealed class RegisterControl : MonoBehaviour {
		public TMP_InputField Login;
		public TMP_InputField Password;

		MainThreadRunner _runner;
		GameSceneManager _scene;
		RegisterService  _register;

		[Inject]
		public void Init(MainThreadRunner runner, GameSceneManager scene, RegisterService register) {
			_runner   = runner;
			_scene    = scene;
			_register = register;

			Login.text    = "test";
			Password.text = "test";
		}
		
		public void Register() {
			_runner.Run(async () => {
				var login    = Login.text;
				var password = Password.text;
				var user     = new UserState(login, password, login);
				var success  = await _register.TryRegister(user);
				if ( success ) {
					_scene.GoToLogin();
				}
			});
		}
	}
}
