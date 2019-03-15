using System.Threading.Tasks;
using GameLogics.Client.Repositories;
using GameLogics.Shared.Dao.Register;
using GameLogics.Shared.Services;

namespace GameLogics.Client.Services {
	public class RegisterService {
		readonly IApiService    _api;
		readonly UserRepository _user;

		public RegisterService(IApiService api, UserRepository user) {
			_api  = api;
			_user = user;
		}
		
		public async Task<bool> TryRegister() {
			var user = _user.CurrentUser;
			if ( user == null ) {
				return false;
			}
			var response = await _api.Post(new RegisterRequest(user.Name, user.Login, user.PasswordHash));
			return response.Success;
		}
	}
}