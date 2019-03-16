using System.Threading.Tasks;
using GameLogics.Client.Models;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class AuthService {
		readonly IApiService        _api;
		readonly INetworkService    _network;
		readonly ClientStateService _state;
		
		public AuthService(IApiService api, INetworkService network, ClientStateService state) {
			_api     = api;
			_network = network;
			_state   = state;
		}
		
		public async Task<bool> TryLogin(User user) {
			if ( user == null ) {
				return false;
			}
			var response = await _api.Post(new AuthRequest(user.Login, user.PasswordHash));
			if ( !response.Success ) {
				return false;
			}
			var result = response.Result;
			
			user.Token = result.Token;
			_network.AuthToken = result.Token;
			_state.User  = user;
			_state.State = result.State;
			
			return true;
		}
	}
}