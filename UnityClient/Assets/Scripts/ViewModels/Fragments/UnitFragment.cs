using TMPro;
using UnityClient.Models;

namespace UnityClient.ViewModels.Fragments {
	public class UnitFragment : BaseFragment {
		public TMP_Text NameText;
		
		public void Init(UnitModel model) {
			NameText.text = $"{model.State.Id} ({model.State.Descriptor})";
		}
	}
}