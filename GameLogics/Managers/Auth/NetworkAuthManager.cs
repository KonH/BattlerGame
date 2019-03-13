using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.Managers.Network;
using GameLogics.Repositories;
using Newtonsoft.Json;

namespace GameLogics.Managers.Auth {
	public class NetworkAuthManager : IAuthManager {
		readonly ICustomLogger   _logger;
		readonly INetworkManager _networkManager;
		readonly UserRepository  _userRepository;
		
		public NetworkAuthManager(ICustomLogger logger, INetworkManager networkManager, UserRepository userRepository) {
			_logger         = logger;
			_networkManager = networkManager;
			_userRepository = userRepository;
		}
		
		public async Task<bool> TryLogin() {
			var user = _userRepository.CurrentUser;
			if ( user == null ) {
				return false;
			}
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