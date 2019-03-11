using System;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Managers;
using GameLogics.Managers.Network;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityClient.Managers {
	public class WebRequestNetworkManager : INetworkManager {
		[Serializable]
		public class Settings {
			public string BaseUrl;
		}

		readonly ICustomLogger _logger;
		readonly Settings      _settings;
		
		public WebRequestNetworkManager(ICustomLogger logger, Settings settings) {
			_logger   = logger;
			_settings = settings;
		}
		
		public async Task<NetworkResponse> PostJson(string relativeUrl, string body) {
			try {
				var data = Encoding.UTF8.GetBytes(body);
				var req  = new UnityWebRequest(_settings.BaseUrl + "api/intent", UnityWebRequest.kHttpVerbPOST);
				req.uploadHandler   = new UploadHandlerRaw(data);
				req.downloadHandler = new DownloadHandlerBuffer();
				req.SetRequestHeader("Accept",       "application/json; charset=UTF-8");
				req.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
				await req.SendWebRequest();
				var isFailed = req.isHttpError || req.isNetworkError;
				return new NetworkResponse((int)req.responseCode, !isFailed, req.downloadHandler.text);
			} catch ( Exception e ) {
				_logger.ErrorFormat("PostJson failed: {0}", e);
				return new NetworkResponse(-1, false, "");
			}
		}
	}
}