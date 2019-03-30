using System;
using TMPro;
using UnityClient.Models;
using UnityEngine.UI;

namespace UnityClient.ViewModels.Fragments {
	public class UnitFragment : BaseFragment {
		public TMP_Text NameText;
		public Button   ActionButton;
		public TMP_Text ActionText;

		UnitModel _model;
		
		public void Init(UnitModel model, string actName, Action<UnitModel> act) {
			_model = model;
			NameText.text = $"{_model.State.Id} ({_model.State.Descriptor})";
			ActionText.text = actName;
			ActionButton.onClick.RemoveAllListeners();
			ActionButton.onClick.AddListener(() => act(_model));
		}
	}
}