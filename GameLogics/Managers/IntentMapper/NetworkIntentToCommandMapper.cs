using System.Collections.Generic;
using System.Threading.Tasks;
using GameLogics.Commands;
using GameLogics.DAO;
using GameLogics.Intents;
using GameLogics.Managers.Network;
using Newtonsoft.Json;

namespace GameLogics.Managers.IntentMapper {
	public class NetworkIntentToCommandMapper : BaseIntentToCommandMapper {
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto
		};

		readonly ICustomLogger _logger;
		
		readonly INetworkManager _networkManager;

		public NetworkIntentToCommandMapper(ICustomLogger logger, IGameStateManager stateManager, INetworkManager networkManager) 
			: base(stateManager) {
			_logger         = logger;
			_networkManager = networkManager;
		}
		
		public override async Task<CommandResponse> RequestCommandsFromIntent(IIntent intent) {
			var body = SerializeIntent(intent);
			var result = await _networkManager.PostJson("api/intent", body);
			if ( result.IsSuccess ) {
				var commands = DeserializeCommands(result.ResponseText);
				return CommandResponse.FromCommands(commands);
			} else {
				_logger.WarningFormat("RequestCommandsFromIntent failed: {0}: {1}", result.StatusCode.ToString(), result.ResponseText);
			}
			return CommandResponse.Failed();
		}
		
		string SerializeIntent(IIntent intent) {
			var request = new IntentRequest { Intent = intent };
			return JsonConvert.SerializeObject(request, _settings);
		}

		List<ICommand> DeserializeCommands(string str) {
			var respose = JsonConvert.DeserializeObject<IntentResponse>(str, _settings);
			return respose.Commands;
		}
	}
}