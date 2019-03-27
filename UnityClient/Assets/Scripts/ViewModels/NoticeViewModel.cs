using System;
using TMPro;
using UnityClient.Models;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UnityClient.ViewModels {
	public class NoticeViewModel : BaseWindowViewModel {
		public float     ShowDuration;
		public float     HideDuration;
		public Transform MessageRoot;
		public TMP_Text  MessageText;
		public Button    OkButton;
		public Button    CloseButton;

		void Awake() {
			MessageRoot.localScale = Vector3.zero;
			MessageRoot.DoScale(ShowDuration, 1.0f).Detach();
		}

		public void Show(NoticeModel model) {
			MessageText.text = model.Message;
			OkButton.onClick.AddListener(() => HideThenCallback(model.Callback, true));
			CloseButton.onClick.AddListener(() => HideThenCallback(model.Callback, false));
		}

		void HideThenCallback(Action<bool> callback, bool result) {
			OkButton.interactable = false;
			CloseButton.interactable = false;
			MessageRoot.DoScale(HideDuration, 0.0f).GetAwaiter()
				.OnCompleted(() => callback(result));
		}
	}
}
