using TMPro;
using UnityClient.Models;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine.UI;

namespace UnityClient.ViewModels.Windows {
	public class NoticeWindow : BaseWindow {
		public TMP_Text  MessageText;
		public Button    OkButton;
		public Button    CloseButton;

		public BaseAnimation Animation;
		
		void Awake() {
			Animation.Show();
		}

		public void Show(NoticeModel model) {
			MessageText.text = model.Message;
			OkButton.onClick.AddListener(() => Animation.Hide(() => model.Callback(true)));
			CloseButton.onClick.AddListener(() => Animation.Hide(() => model.Callback(false)));
		}
	}
}
