using TMPro;
using UnityClient.Models;

namespace UnityClient.ViewModels.Fragments {
	public class ItemFragment : BaseFragment {
		public TMP_Text NameText;
		
		public void Init(ItemModel model) {
			NameText.text = $"{model.State.Descriptor} ({model.Config.Type})";
		}
	}
}