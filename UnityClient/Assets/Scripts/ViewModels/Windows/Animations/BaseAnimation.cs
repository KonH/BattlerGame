using System;
using UnityEngine;
using UnityClient.Utils;

namespace UnityClient.ViewModels.Windows.Animations {
	public abstract class BaseAnimation : MonoBehaviour {
		public float     ShowDuration;
		public float     HideDuration;
		public Transform Root;

		public void Show(Action callback = null) => PerformShow().GetAwaiter().OnCompleted(callback);
		
		public void Hide(Action callback = null) { 
			var canvasGroup = GetComponent<CanvasGroup>();
			if ( canvasGroup ) {
				canvasGroup.interactable = false;
			}
			PerformHide().GetAwaiter().OnCompleted(callback);
		}

		public abstract AsyncExtensions.UpdateHelper PerformShow();
		public abstract AsyncExtensions.UpdateHelper PerformHide();
	}
}