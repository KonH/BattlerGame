using System;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine.UI;

namespace UnityClient.ViewModels.Windows {
	public class LoseWindow : BaseWindow {
		public Button OkButton;

		public BaseAnimation Animation;
		
		void Awake() {
			Animation.Show();
		}

		public void Show(Action callback) {
			OkButton.onClick.AddListener(() => Animation.Hide(callback));
		}
	}
}
