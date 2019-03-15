using System.Threading.Tasks;
using GameLogics.Client.Repositories;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class AuthService {
		readonly IApiService         _api;
		readonly INetworkService     _network;
		readonly UserRepository      _user;
		readonly GameStateRepository _state;
		
		public AuthService(IApiService api, INetworkService network, UserRepository user, GameStateRepository state) {
			_api     = api;
			_network = network;
			_user    = user;
			_state   = state;
		}
		
		public async Task<bool> TryLogin() {
			var user = _user.CurrentUser;
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
			_state.State = result.State;
			
			return true;
		}
	}
}