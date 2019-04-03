using System;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;

namespace UnityClient.ViewModels.Windows {
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
		
		public void Hide(Action callback = null) {
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