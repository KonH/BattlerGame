using GameLogics.Server.Models;
using GameLogics.Server.Repositories.Users;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Register;

namespace GameLogics.Server.Services {
	public class RegisterService {
		readonly IUsersRepository _users;

		public RegisterService(IUsersRepository users) {
			_users = users;
		}
		
		public ApiResponse<RegisterResponse> RegisterNewUser(RegisterRequest req) {
			if ( string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Login) || string.IsNullOrWhiteSpace(req.PasswordHash) ) {
				return new ClientError("Invalid user").AsError<RegisterResponse>();
			}
			var user = new User(req.Login, req.PasswordHash, req.Name, "user");
			if ( !_users.TryAdd(user) ) {
				return new ServerError().AsError<RegisterResponse>();
			}
			return new RegisterResponse().AsResult();
		}
	}
}