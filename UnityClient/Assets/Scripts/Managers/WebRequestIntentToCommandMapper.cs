using System;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using UnityClient.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityClient.Managers {
	public class WebRequestIntentToCommandMapper : BaseIntentToCommandMapper {
		[Serializable]
		public class Settings {
			public string BaseUrl;
		}

		readonly Settings _settings;
		
		public WebRequestIntentToCommandMapper(Settings settings, IGameStateManager stateManager) : base(stateManager) {
			_settings = settings;
		}

		public override async Task<CommandResponse> RequestCommandsFromIntent(IIntent intent) {
			try {
				var body = SerializeIntent(intent);
				var data = Encoding.UTF8.GetBytes(body);
				var req  = new UnityWebRequest(_settings.BaseUrl + "api/intent", UnityWebRequest.kHttpVerbPOST);
				req.uploadHandler   = new UploadHandlerRaw(data);
				req.downloadHandler = new DownloadHandlerBuffer();
				req.SetRequestHeader("Accept",       "application/json; charset=UTF-8");
				req.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
				await req.SendWebRequest();
				if ( req.isHttpError || req.isNetworkError ) {
					Debug.LogError($"Something went wrong ({req.uri}): {req.error} ({req.responseCode}): {req.downloadHandler.text}");
					return CommandResponse.Failed();
				}
				var commands = DeserializeCommands(req.downloadHandler.text);
				return CommandResponse.FromCommands(commands);
			} catch ( Exception e ) {
				Debug.LogError(e);
				return CommandResponse.Failed();
			}
		}
	}
}