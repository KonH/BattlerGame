using System;
using UnityClient.Services;
using UnityClient.ViewModels;
using UnityEngine;
using Zenject;

namespace UnityClient.Managers {
	public class UiManager : ITickable {
		[Serializable]
		public class Settings {
			public Canvas Canvas = null;
		}
		
		readonly NoticeService              _service;
		readonly Settings                   _settings;
		readonly BaseWindowViewModelFactory _factory;

		public UiManager(NoticeService service, Settings settings, BaseWindowViewModelFactory factory) {
			_service  = service;
			_settings = settings;
			_factory  = factory;
		}
		
		public void Tick() {
			var notice = _service.RequestNotice();
			if ( notice != null ) {
				ShowWindow<NoticeViewModel>(w => w.Show(notice));
			}
		}
		
		public void ShowWindow<T>(Action<T> init) where T : BaseWindowViewModel {
			var instance = _factory.Create(typeof(T)) as T;
			Debug.Assert(instance != null);
			init(instance);
			instance.transform.SetParent(_settings.Canvas.transform, false);
		}
	}
}
