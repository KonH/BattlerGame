using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityClient.Utils {
	public static class AsyncExtensions {
		static AsyncHandler           _handler  = null;
		static HashSet<UpdateAwaiter> _awaiters = new HashSet<UpdateAwaiter>();
		static HashSet<UpdateAwaiter> _done     = new HashSet<UpdateAwaiter>();
		
		class AsyncHandler : MonoBehaviour {
			void Update() {
				foreach ( var awaiter in _awaiters ) {
					if ( awaiter.Update() ) {
						_done.Add(awaiter);
					}
				}
				foreach ( var awaiter in _done ) {
					awaiter.Finish();
				}
				foreach ( var awaiter in _done ) {
					_awaiters.Remove(awaiter);
				}
				_done.Clear();
			}
		}

		public class UpdateAwaiter : INotifyCompletion {
			public bool IsCompleted { get; private set; }
		
			readonly Action<float> _onProgress;

			Action _onFinish     = null;
			Action _continuation = null;

			float _speed    = 0.0f;
			float _progress = 0.0f;
		
			public UpdateAwaiter(float speed, Action<float> onProgress, Action onFinish) {
				_speed      = speed;
				_onProgress = onProgress;
				_onFinish   = onFinish;
			}
		
			public void OnCompleted(Action continuation) {
				_continuation = continuation;
			}

			public bool Update() {
				_progress += Time.deltaTime * _speed;
				_onProgress?.Invoke(_progress);
				return _progress >= 1.0f;
			}

			public void Finish() {
				_onFinish?.Invoke();
				IsCompleted = true;
				_continuation?.Invoke();
			}
		
			public void GetResult() {}
		}
		
		public struct UpdateHelper {
			UpdateAwaiter _awaiter;

			public UpdateHelper(float speed, Action<float> onProgress, Action onFinish) {				
				_awaiter = new UpdateAwaiter(speed, onProgress, onFinish);
				_awaiters.Add(_awaiter);
			}
			
			public UpdateAwaiter GetAwaiter() {
				return _awaiter;
			}
		}
		
		static void EnsureHandler() {
			if ( _handler == null ) {
				var go = new GameObject("[AsyncHandler]");
				GameObject.DontDestroyOnLoad(go);
				_handler = go.AddComponent<AsyncHandler>();
			}
		}

		public static UpdateHelper Do(float duration, Action<float> onProgress, Action onFinish) {
			EnsureHandler();
			return new UpdateHelper(1 / duration, onProgress, onFinish);
		}

		public static UpdateHelper DoScale(this Transform transform, float duration, Vector3 endValue) {
			var startValue = transform.localScale;
			return Do(duration, p => transform.localScale = Vector3.Lerp(startValue, endValue, p), () => transform.localScale = endValue);
		}
		
		public static UpdateHelper DoScale(this Transform transform, float duration, float endValue) {
			return transform.DoScale(duration, Vector3.one * endValue);
		}

		public static UpdateHelper Wait(this MonoBehaviour _, float duration) {
			return Do(duration, null, null);
		}
	}
}