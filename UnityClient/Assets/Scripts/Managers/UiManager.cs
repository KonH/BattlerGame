using System;
using UnityClient.Models;
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
		
		readonly NoticeService           _service;
		readonly Settings                _settings;
		readonly NoticeViewModel.Factory _noticeFactory;

		public UiManager(NoticeService service, Settings settings, NoticeViewModel.Factory noticeFactory) {
			_service       = service;
			_settings      = settings;
			_noticeFactory = noticeFactory;
		}
		
		public void Tick() {
			var notice = _service.RequestNotice();
			if ( notice != null ) {
				ShowNotice(notice);
			}
		}

		void ShowNotice(NoticeModel notice) {
			var instance = _noticeFactory.Create(notice);
			instance.transform.SetParent(_settings.Canvas.transform, false);
		}
	}
}
