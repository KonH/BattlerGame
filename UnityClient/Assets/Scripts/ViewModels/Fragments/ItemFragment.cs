using System;
using TMPro;
using UnityClient.Models;
using UnityEngine.UI;

namespace UnityClient.ViewModels.Fragments {
	public class ItemFragment : BaseFragment {
		public TMP_Text NameText;
		public Button   ActionButton;
		public TMP_Text ActionText;

		ItemModel _model;
		
		public void Init(ItemModel model, string actName, Action<ItemModel> act) {
			_model = model;
			NameText.text = !model.IsFake ? $"{model.State.Descriptor} ({model.Config.Type})" : model.FakeType.ToString();
			ActionButton.gameObject.SetActive(act != null);
			if ( act != null ) {
				ActionButton.onClick.RemoveAllListeners();
				ActionButton.onClick.AddListener(() => act(_model));
				ActionText.text = actName;
			}
		}
	}
}