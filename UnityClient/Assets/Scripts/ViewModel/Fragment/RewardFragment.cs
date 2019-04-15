using TMPro;
using UnityClient.ViewModel.Window.Animation;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModel.Fragment {
	public sealed class RewardFragment : BaseFragment {		
		public sealed class Factory : PlaceholderFactory<Transform, string, RewardFragment> {}
		
		public TMP_Text      Text;
		public BaseAnimation Animation;

		[Inject]
		public void Init(Transform parent, string text) {
			Text.text = text;
			
			Attach(parent);
		}
	}
}