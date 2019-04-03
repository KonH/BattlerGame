using System;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class LoseWindow : BaseWindow {
		public class Factory : PlaceholderFactory<Action, LoseWindow> {}
		
		public Button OkButton;


		[Inject]
		public void Init(Canvas parent, Action callback) {
			OkButton.onClick.AddListener(() => Hide(callback));
			
			ShowAt(parent);
		}
	}
}
