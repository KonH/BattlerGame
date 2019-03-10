using System;
using System.Text;
using System.Threading.Tasks;
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

		readonly Settings _settings;
		
		public WebRequestNetworkManager(Settings settings) {
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
				Debug.LogError(e);
				return new NetworkResponse(-1, true, e.ToString());
			}
		}
	}
}