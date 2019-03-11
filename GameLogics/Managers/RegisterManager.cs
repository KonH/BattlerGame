using System.Threading.Tasks;
using GameLogics.Managers.Network;
using GameLogics.Models;
using Newtonsoft.Json;

namespace GameLogics.Managers {
	public class RegisterManager {
		readonly ICustomLogger   _logger;
		readonly INetworkManager _networkManager;

		public RegisterManager(ICustomLogger logger, INetworkManager networkManager) {
			_logger         = logger;
			_networkManager = networkManager;
		}
		
		public async Task<bool> TryRegister(User user) {
			var body = JsonConvert.SerializeObject(user);
			var result = await _networkManager.PostJson("api/user", body);
			_logger.DebugFormat("TryRegister: {0}", result);
			return result.IsSuccess;
		}
	}
}