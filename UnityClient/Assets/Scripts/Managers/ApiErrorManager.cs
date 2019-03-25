using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Services;
using UnityClient.Managers;
using UnityClient.Models;

namespace UnityClient.Services {
	public class ApiErrorManager {
		readonly NoticeService    _notice;
		readonly GameSceneManager _scene;
		
		public ApiErrorManager(IApiService api, NoticeService notice, GameSceneManager scene) {
			_notice = notice;
			_scene  = scene;
			
			api.OnError += OnApiError;
		}

		void OnApiError(IApiError obj) {
			var message = !string.IsNullOrEmpty(obj.Message) ? obj.Message : obj.GetType().Name;
			_notice.ScheduleNotice(new NoticeModel(message, _ => _scene.GoToStart()));
		}
	}
}