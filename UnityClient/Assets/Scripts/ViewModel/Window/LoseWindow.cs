using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModel.Window {
	public sealed class LoseWindow : BaseWindow {
		public sealed class Factory : PlaceholderFactory<Action, LoseWindow> {}
		
		public Button OkButton;


		[Inject]
		public void Init(Canvas parent, Action callback) {
			OkButton.onClick.AddListener(() => Hide(callback));
			
			ShowAt(parent);
		}
	}
}
