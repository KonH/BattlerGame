using TMPro;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;
using Zenject;

namespace UnityClient.ViewModels.Fragments {
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