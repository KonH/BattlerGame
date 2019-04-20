using System;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Client.Service;
using GameLogics.Client.Utils;
using GameLogics.Shared.Service;
using UnityEngine.Networking;
using UnityClient.Utils;

namespace UnityClient.Service {
	public sealed class WebRequestNetworkService : INetworkService {
		readonly ICustomLogger  _logger;
		readonly ServerSettings _settings;
		
		public string AuthToken { get; set; }
		
		public WebRequestNetworkService(ICustomLogger logger, ServerSettings settings) {
			_logger   = logger;
			_settings = settings;
		}
		
		public async Task<NetworkResponse> PostJson(string relativeUrl, string body) {
			try {
				var data = Encoding.UTF8.GetBytes(body);
				var req  = new UnityWebRequest(_settings.BaseUrl + relativeUrl, UnityWebRequest.kHttpVerbPOST);				
				if ( !string.IsNullOrEmpty(AuthToken) ) {
					req.SetRequestHeader("Authorization", "Bearer " + AuthToken);
				}
				req.uploadHandler   = new UploadHandlerRaw(data);
				req.downloadHandler = new DownloadHandlerBuffer();
				req.SetRequestHeader("Accept",       "application/json; charset=UTF-8");
				req.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
				await req.SendWebRequest();
				var isFailed = req.isHttpError || req.isNetworkError;
				return new NetworkResponse(!isFailed, req.downloadHandler.text, (int)req.responseCode);
			} catch ( Exception e ) {
				_logger.ErrorFormat(this, "PostJson failed: {0}", e);
				return new NetworkResponse(false, "", -1);
			}
		}
	}
}