using System.Threading.Tasks;
using GameLogics.Managers.Auth;
using GameLogics.Models;
using TMPro;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UnityClient.Controls {
	public class LoginControl : MonoBehaviour {
		public TMP_InputField LoginInput;
		public TMP_InputField PasswordInput;

		IAuthManager _authManager;
		
		[Inject]
		public void Init(IAuthManager authManager) {
			_authManager = authManager;
			LoginInput.text = "test";
			PasswordInput.text = "test";
		}

		public void Login() {
			var login = LoginInput.text;
			var password = PasswordInput.text;
			_authManager.TryLogin(User.CreateWithPassword(login, password, login, "user")).ContinueOnSameThread(GoToWorld);
		}

		void GoToWorld(Task<bool> result) {
			if ( result.Result ) {
				SceneManager.LoadScene(3);
			}
		}
	}
}
