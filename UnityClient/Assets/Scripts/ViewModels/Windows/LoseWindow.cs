using System;
using TMPro;
using UnityClient.Models;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class LoseWindow : BaseWindow {
		public float     ShowDuration;
		public float     HideDuration;
		public Transform MessageRoot;
		public Button    OkButton;
		
		void Awake() {
			MessageRoot.localScale = Vector3.zero;
			MessageRoot.DoScale(ShowDuration, 1.0f).Detach();
		}

		[Inject]
		public void Init() {
			
		}

		public void Show(Action callback) {
			OkButton.onClick.AddListener(() => HideThenClose(callback));
		}

		void HideThenClose(Action callback) {
			OkButton.interactable = false;
			MessageRoot.DoScale(HideDuration, 0.0f).GetAwaiter()
				.OnCompleted(callback);
		}
	}
}
