using System;
using System.Collections.Generic;
using UnityClient.Managers;
using UnityClient.ViewModels.Fragments;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using Zenject;

namespace UnityClient.Installers {
	public class UiSetupInstaller : ScriptableObjectInstaller {
		public List<BaseWindow>   Windows;
		public List<BaseFragment> Fragments;
		
		public override void InstallBindings() {
			BindFactory<BaseWindow,   BaseWindowFactory>  (Windows);
			BindFactory<BaseFragment, BaseFragmentFactory>(Fragments);
		}

		void BindFactory<TElement, TFactory>(List<TElement> elements) where TElement : MonoBehaviour where TFactory : PlaceholderFactory<Type, TElement> {
			var dict = new Dictionary<Type, TElement>();
			foreach ( var element in elements ) {
				dict[element.GetType()] = element;
			}
			Container.BindFactory<Type, TElement, TFactory>().FromMethod((_, type) => CreateFromPrefab(type, dict));
		}

		T CreateFromPrefab<T>(Type type, Dictionary<Type, T> prefabs) where T : MonoBehaviour {
			var prefab = prefabs[type];
			return Instantiate(prefab);
		}
	}
}
