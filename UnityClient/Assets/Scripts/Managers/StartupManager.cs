using System;
using GameLogics.Client.Services;
using UnityClient.Services;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class StartupManager : IInitializable, ITickable {
		const float _minInterval = 3.0f;

		readonly MainThreadRunner   _runner;
		readonly ServerSettings     _settings;
		readonly AuthService        _auth;
		readonly GameSceneManager   _scene;
		readonly ClientStateService _state;

		float _nextRefreshTime = _minInterval;
		
		bool IsNeedToRefreshToken => (Time.realtimeSinceStartup > _nextRefreshTime) && !_scene.IsLoginOrRegister;
		
		public StartupManager(MainThreadRunner runner, ServerSettings settings, AuthService auth, GameSceneManager scene, ClientStateService state) {
			_runner   = runner;
			_settings = settings;
			_auth     = auth;
			_scene    = scene;
			_state    = state;
			
			_nextRefreshTime = _minInterval;
		}
		
		public void Initialize() {
			TryLogin();
		}

		public void Tick() {
			if ( IsNeedToRefreshToken ) {
				TryLogin();
			}
		}
		
		void TryLogin() {
			_runner.Run(async () => {
				var user = _state.User;
				if ( user == null ) {
					_scene.GoToLogin();
					return;
				}
				var isLoginSuccess = await _auth.TryLogin(user);
				var interval = Math.Max(isLoginSuccess ? _settings.TokenRefreshInterval : _minInterval, _minInterval);
				_nextRefreshTime = Time.realtimeSinceStartup + interval;
				if ( isLoginSuccess ) {
					_scene.GoToWorld();
				} else {
					_scene.GoToLogin();
				}
			});
		}
	}
}
