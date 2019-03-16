using System.Threading.Tasks;
using GameLogics.Client.Models;
using GameLogics.Shared.Dao.Register;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class RegisterService {
		readonly IApiService        _api;
		readonly ClientStateService _state;

		public RegisterService(IApiService api, ClientStateService state) {
			_api   = api;
			_state = state;
		}
		
		public async Task<bool> TryRegister(User user) {
			if ( user == null ) {
				return false;
			}
			var response = await _api.Post(new RegisterRequest(user.Name, user.Login, user.PasswordHash));
			if ( !response.Success ) {
				return false;
			}
			_state.User = user;
			return true;
		}
	}
}