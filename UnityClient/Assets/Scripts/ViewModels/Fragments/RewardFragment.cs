using TMPro;
using UnityClient.ViewModels.Windows.Animations;

namespace UnityClient.ViewModels.Fragments {
	public class RewardFragment : BaseFragment {
		public TMP_Text      Text;
		public BaseAnimation Animation;

		public void Init(string text) {
			Text.text = text;
		}
	}
}