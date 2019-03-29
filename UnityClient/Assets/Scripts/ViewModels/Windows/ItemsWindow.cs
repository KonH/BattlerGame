using System.Collections.Generic;
using UnityClient.Models;
using UnityClient.ViewModels.Fragments;
using UnityEngine.UI;
using UnityClient.ViewModels.Windows.Animations;
using UnityEngine;

namespace UnityClient.ViewModels.Windows {
	public class ItemsWindow : BaseWindow {
		public Button    CloseButton;
		public Transform ItemsRoot;

		public BaseAnimation Animation;
		
		void Awake() {
			Animation.Show();
		}

		public void Show(List<ItemModel> items, ItemFragment itemTemplate) {
			CloseButton.onClick.AddListener(() => Animation.Hide(() => Destroy(gameObject)));
			itemTemplate.transform.SetParent(ItemsRoot, false);
			foreach ( var item in items ) {
				var instance = Instantiate(itemTemplate, ItemsRoot, false);
				instance.Init(item);
			}
			itemTemplate.gameObject.SetActive(false);
		}
	}
}
