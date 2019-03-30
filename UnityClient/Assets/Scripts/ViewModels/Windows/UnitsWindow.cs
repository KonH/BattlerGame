using System;
using System.Collections.Generic;
using UnityClient.Models;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;

namespace UnityClient.ViewModels.Windows {
	public class UnitsWindow : BaseWindow {
		public Button    CloseButton;
		public Transform ItemsRoot;

		public BaseAnimation Animation;
		
		void Awake() {
			Animation.Show();
		}

		public void Show(List<UnitModel> units, UnitFragment unitTemplate, string actName, Action<UnitModel> act) {
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			unitTemplate.transform.SetParent(ItemsRoot, false);
			foreach ( var unit in units ) {
				var instance = Instantiate(unitTemplate, ItemsRoot, false);
				instance.Init(unit, actName, act);
			}
			unitTemplate.gameObject.SetActive(false);
		}
	}
}
