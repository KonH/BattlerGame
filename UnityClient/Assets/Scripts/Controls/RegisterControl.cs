using GameLogics.Managers;
using GameLogics.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace UnityClient.Controls {
	public class RegisterControl : MonoBehaviour {
		public TMP_InputField Login;
		public TMP_InputField Password;

		RegisterManager _registerManager;
		
		[Inject]
		public void Init(RegisterManager registerManager) {
			_registerManager = registerManager;
		}
		
		public void Register() {
			var login = Login.text;
			var password = Password.text;
			var user = User.CreateWithPassword(login, password, login, "user");
			_registerManager.TryRegister(user);
		}
	}
}
