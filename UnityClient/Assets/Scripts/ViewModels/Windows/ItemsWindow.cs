using UnityEngine.UI;
using UnityClient.ViewModels.Windows.Animations;

namespace UnityClient.ViewModels.Windows {
	public class ItemsWindow : BaseWindow {
		public Button CloseButton;

		public BaseAnimation Animation;
		
		void Awake() {
			Animation.Show();
		}

		public void Show() {
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
		}
	}
}
