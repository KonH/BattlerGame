using System;
using UnityClient.Services;
using UnityClient.ViewModels.Windows;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class UiManager : ITickable {
		[Serializable]
		public class Settings {
			public Canvas Canvas = null;
		}
		
		readonly NoticeService     _service;
		readonly Settings          _settings;
		readonly BaseWindowFactory _factory;

		public UiManager(NoticeService service, Settings settings, BaseWindowFactory factory) {
			_service  = service;
			_settings = settings;
			_factory  = factory;
		}
		
		public void Tick() {
			var notice = _service.RequestNotice();
			if ( notice != null ) {
				ShowWindow<NoticeWindow>(w => w.Show(notice));
			}
		}
		
		public void ShowWindow<T>(Action<T> init) where T : BaseWindow {
			var instance = _factory.Create(typeof(T)) as T;
			Debug.Assert(instance != null);
			init(instance);
			instance.transform.SetParent(_settings.Canvas.transform, false);
		}
	}
}
