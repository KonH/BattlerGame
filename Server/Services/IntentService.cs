using System.Threading.Tasks;
using GameLogics.Intents;
using GameLogics.Managers.IntentMapper;
using Microsoft.Extensions.Logging;

namespace Server.Services {
	public class IntentService {
		readonly ILogger                     _logger;
		readonly DirectIntentToCommandMapper _mapper;

		public IntentService(ILogger<IntentService> logger, DirectIntentToCommandMapper mapper) {
			_logger = logger;
			_mapper = mapper;
		}
		
		public async Task<CommandResponse> CreateCommands(string userName, IIntent intent) {
			var response = await _mapper.RequestCommandsFromIntent(intent);
			return response;
		}
	}
}