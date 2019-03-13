using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.DAO.Errors;
using GameLogics.Intents;
using GameLogics.Managers.Network;
using GameLogics.Models;
using GameLogics.Repositories.State;
using Newtonsoft.Json;

namespace GameLogics.Managers.IntentMapper {
	public class NetworkIntentToCommandMapper : BaseIntentToCommandMapper {
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto
		};

		readonly ICustomLogger        _logger;
		readonly INetworkManager      _networkManager;
		readonly IGameStateRepository _stateRepository;

		public NetworkIntentToCommandMapper(ICustomLogger logger, INetworkManager networkManager, IGameStateRepository stateRepository) {
			_logger          = logger;
			_networkManager  = networkManager;
			_stateRepository = stateRepository;
		}
		
		public override async Task<CommandResponse> RequestCommandsFromIntent(GameState state, IIntent intent) {
			var body = SerializeIntent(intent);
			var result = await _networkManager.PostJson("api/intent", body);
			_logger.DebugFormat("RequestCommandsFromIntent: '{0}'", result.ResponseText);
			if ( result.IsSuccess ) {
				var response = DeserializeResponse(result.ResponseText);
				_stateRepository.Version = response.Version;
				return response;
			}
			_logger.WarningFormat("RequestCommandsFromIntent failed: {0}: '{1}'", result.StatusCode.ToString(), result.ResponseText);
			return CommandResponse.Failed(new NetworkError());
		}
		
		string SerializeIntent(IIntent intent) {
			var expectedVersion = _stateRepository.Version;
			var request = new IntentRequest(expectedVersion, intent);
			return JsonConvert.SerializeObject(request, _settings);
		}

		CommandResponse DeserializeResponse(string str) {
			return JsonConvert.DeserializeObject<CommandResponse>(str, _settings);
		}
	}
}