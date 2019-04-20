using GameLogics.Server.Model;
using GameLogics.Server.Repository.User;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Dao.Register;

namespace GameLogics.Server.Service {
	public sealed class RegisterService {
		readonly IUserRepository _users;

		public RegisterService(IUserRepository users) {
			_users = users;
		}
		
		public ApiResponse<RegisterResponse> RegisterNewUser(RegisterRequest req) {
			if ( string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Login) || string.IsNullOrWhiteSpace(req.PasswordHash) ) {
				return new ClientError("Invalid user").AsError<RegisterResponse>();
			}
			if ( _users.Find(req.Login) != null ) {
				return new ClientError("Name already in use").AsError<RegisterResponse>();
			}
			var user = new UserState(req.Login, req.PasswordHash, req.Name, "user");
			if ( !_users.TryAdd(user) ) {
				return new ServerError().AsError<RegisterResponse>();
			}
			return new RegisterResponse().AsResult();
		}
	}
}