using TMPro;
using UnityClient.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels.Windows {
	public class NoticeWindow : BaseWindow {
		public class Factory : PlaceholderFactory<NoticeModel, NoticeWindow> {}
		
		public TMP_Text  MessageText;
		public Button    OkButton;
		public Button    CloseButton;

		[Inject]
		public void Init(Canvas parent, NoticeModel model) {			
			MessageText.text = model.Message;
			OkButton.onClick.AddListener(() => Hide(() => model.Callback(true)));
			CloseButton.onClick.AddListener(() => Hide(() => model.Callback(false)));
			
			ShowAt(parent);
		}
	}
}
