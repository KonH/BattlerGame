using System.Threading.Tasks;
using GameLogics.Client.Model;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Service;
using GameLogics.Shared.Service.Time;

namespace GameLogics.Client.Service {
	public sealed class AuthService {
		readonly IApiService        _api;
		readonly INetworkService    _network;
		readonly OffsetTimeService  _offsetTime;
		readonly ClientStateService _state;
		
		public AuthService(IApiService api, INetworkService network, OffsetTimeService offsetTime, ClientStateService state) {
			_api        = api;
			_network    = network;
			_offsetTime = offsetTime;
			_state      = state;
		}
		
		public async Task<bool> TryLogin(UserState user) {
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
			_state.Config = result.Config;

			_offsetTime.MarkOrigin();

			return true;
		}
	}
}