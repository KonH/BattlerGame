using System.Threading.Tasks;
using GameLogics.Managers.Network;
using GameLogics.Models;
using GameLogics.Repositories;
using Newtonsoft.Json;

namespace GameLogics.Managers {
	public class RegisterManager {
		readonly ICustomLogger   _logger;
		readonly INetworkManager _networkManager;
		readonly UserRepository  _userRepository;

		public RegisterManager(ICustomLogger logger, INetworkManager networkManager, UserRepository userRepository) {
			_logger         = logger;
			_networkManager = networkManager;
			_userRepository = userRepository;
		}
		
		public async Task<bool> TryRegister() {
			var user = _userRepository.CurrentUser;
			if ( user == null ) {
				return false;
			}
			var body = JsonConvert.SerializeObject(user);
			var result = await _networkManager.PostJson("api/user", body);
			_logger.DebugFormat("TryRegister: {0}", result);
			return result.IsSuccess;
		}
	}
}