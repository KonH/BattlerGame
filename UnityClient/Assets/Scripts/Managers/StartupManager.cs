using System;
using GameLogics.Managers.Auth;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class StartupManager : IInitializable, ITickable {
		const float _minInterval = 3.0f;
		
		readonly MainThreadRunner _runner;
		readonly ServerSettings   _settings;
		readonly IAuthManager     _authManager;
		readonly GameSceneManager _sceneManager;

		float _nextRefreshTime = _minInterval;
		
		bool IsNeedToRefreshToken => (Time.realtimeSinceStartup > _nextRefreshTime) && !_sceneManager.IsRegister;
		
		public StartupManager(MainThreadRunner runner, ServerSettings settings, IAuthManager authManager, GameSceneManager sceneManager) {
			_runner       = runner;
			_settings     = settings;
			_authManager  = authManager;
			_sceneManager = sceneManager;
			
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
				var isLoginSuccess = await _authManager.TryLogin();
				var interval = Math.Max(isLoginSuccess ? _settings.TokenRefreshInterval : _minInterval, _minInterval);
				_nextRefreshTime = Time.realtimeSinceStartup + interval;
				if ( isLoginSuccess ) {
					_sceneManager.GoToWorld();
				} else {
					_sceneManager.GoToLogin();
				}
			});
		}
	}
}
