using UnityClient.Service;
using UnityClient.ViewModel.Window;
using Zenject;

namespace UnityClient.Manager {
	public sealed class NoticeManager : ITickable {		
		readonly NoticeService        _service;
		readonly NoticeWindow.Factory _window;

		public NoticeManager(NoticeService service, NoticeWindow.Factory window) {
			_service = service;
			_window  = window;
		}
		
		public void Tick() {
			var notice = _service.RequestNotice();
			if ( notice != null ) {
				_window.Create(notice);
			}
		}
	}
}
