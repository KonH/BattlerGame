using System;
using UnityClient.ViewModel.Window.Animation;
using UnityEngine;

namespace UnityClient.ViewModel.Window {
	public abstract class BaseWindow : MonoBehaviour {
		public BaseAnimation Animation;

		protected void ShowAt(Canvas canvas, Action callback = null) {
			transform.SetParent(canvas.transform, false);
			Show(callback);
		}
		
		protected void Show(Action callback = null) {
			if ( Animation ) {
				Animation.Show(callback);
			}
		}

		public void Hide() => Hide(null);
		
		public void Hide(Action callback) {
			if ( Animation ) {
				Animation.Hide(() => {
					OnHide(callback);
				});
			} else {
				OnHide(callback);
			}
		}

		void OnHide(Action callback) {
			Destroy(gameObject);
			callback?.Invoke();
		}
	}
}