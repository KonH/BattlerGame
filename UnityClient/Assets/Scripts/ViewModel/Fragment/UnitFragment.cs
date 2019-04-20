using TMPro;
using UnityClient.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModel.Fragment {
	public sealed class UnitFragment : BaseFragment {		
		public sealed class Factory : PlaceholderFactory<Transform, UnitModel, UnitFragment> {}
		
		public TMP_Text NameText;
		public Button   ActionButton;
		public TMP_Text ActionText;
		
		[Inject]
		public void Init(Transform parent, UnitModel model) {
			Attach(parent);
			Refresh(model);
		}

		public void Refresh(UnitModel model) {
			NameText.text = model.Name;
			ActionButton.gameObject.SetActive(model.HasAction);
			if ( model.HasAction ) {
				ActionText.text = model.ActionName;
				ActionButton.onClick.RemoveAllListeners();
				ActionButton.onClick.AddListener(model.Click);
			}
		}
	}
}