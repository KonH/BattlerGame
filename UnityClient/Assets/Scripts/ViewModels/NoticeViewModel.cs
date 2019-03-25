using TMPro;
using UnityClient.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels {
	public class NoticeViewModel : MonoBehaviour {
		public class Factory : PlaceholderFactory<NoticeModel, NoticeViewModel> {}

		public TMP_Text MessageText;
		public Button   OkButton;
		public Button   CloseButton;
		
		[Inject]
		public void Init(NoticeModel model) {
			MessageText.text = model.Message;
			OkButton.onClick.AddListener(() => model.Callback(true));
			CloseButton.onClick.AddListener(() => model.Callback(false));
		}
	}
}
