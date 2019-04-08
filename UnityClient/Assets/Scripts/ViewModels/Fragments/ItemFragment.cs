using TMPro;
using UnityClient.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UnityClient.ViewModels.Fragments {
	public sealed class ItemFragment : BaseFragment {		
		public sealed class Factory : PlaceholderFactory<Transform, ItemModel, ItemFragment> {}
		
		public TMP_Text NameText;
		public Button   ActionButton;
		public TMP_Text ActionText;
		
		[Inject]
		public void Init(Transform parent, ItemModel model) {
			Attach(parent);
			Refresh(model);
		}

		public void Refresh(ItemModel model) {
			NameText.text = model.Name;
			ActionButton.gameObject.SetActive(model.HasAction);
			if ( model.HasAction ) {
				ActionButton.onClick.RemoveAllListeners();
				ActionButton.onClick.AddListener(model.Click);
				ActionText.text = model.ActionName;
			}
		}
	}
}