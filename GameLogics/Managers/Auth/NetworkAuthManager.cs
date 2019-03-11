using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.Managers.Network;
using GameLogics.Models;
using Newtonsoft.Json;

namespace GameLogics.Managers.Auth {
	public class NetworkAuthManager : IAuthManager {
		readonly ICustomLogger   _logger;
		readonly INetworkManager _networkManager;
		
		public NetworkAuthManager(ICustomLogger logger, INetworkManager networkManager) {
			_logger         = logger;
			_networkManager = networkManager;
		}
		
		public async Task<bool> TryLogin(User user) {
			var body = JsonConvert.SerializeObject(user);
			var result = await _networkManager.PostJson("api/auth", body);
			_logger.DebugFormat("TryLogin: {0}, {1}", result.IsSuccess.ToString(), result.ResponseText);
			if ( result.IsSuccess ) {
				var response = JsonConvert.DeserializeObject<AuthResponse>(result.ResponseText);
				_logger.DebugFormat("TryLogin: {0}", response);
				_networkManager.AuthToken = response.Token;
			}
			return result.IsSuccess;
		}
	}
}