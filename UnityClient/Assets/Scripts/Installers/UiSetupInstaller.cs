using System;
using System.Collections.Generic;
using UnityClient.Managers;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using Zenject;

namespace UnityClient.Installers {
	public class UiSetupInstaller : ScriptableObjectInstaller {
		public List<BaseWindow> Windows;
		
		public override void InstallBindings() {
			Container.BindFactory<Type, BaseWindow, BaseWindowFactory>().FromMethod(CreateWindow);
		}

		BaseWindow CreateWindow(DiContainer container, Type type) {
			BaseWindow prefab = null;
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
