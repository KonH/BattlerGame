using System;
using System.Collections.Generic;
using UnityClient.Managers;
using UnityClient.ViewModels;
using UnityEngine;
using Zenject;

namespace UnityClient.Installers {
	public class UiSetupInstaller : ScriptableObjectInstaller {
		public List<BaseWindowViewModel> Windows;
		
		public override void InstallBindings() {
			Container.BindFactory<Type, BaseWindowViewModel, BaseWindowViewModelFactory>().FromMethod(CreateWindow);
		}

		BaseWindowViewModel CreateWindow(DiContainer container, Type type) {
			BaseWindowViewModel prefab = null;
			foreach ( var window in Windows ) {
				if ( window.GetType() == type ) {
					prefab = window;
					break;
				}
			}
			Debug.Assert(prefab != null);
			return Instantiate(prefab);
		}
	}
}
