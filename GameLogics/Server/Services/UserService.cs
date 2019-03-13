using GameLogics.Managers;
using GameLogics.Models;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Utils.Api;
using GameLogics.Server.Utils.Api.Errors;

namespace GameLogics.Server.Services {
	public class UserService {
		readonly ICustomLogger    _logger;
		readonly ApiService       _api;
		readonly IUsersRepository _users;

		public UserService(ICustomLogger logger, ApiService api, IUsersRepository users) {
			_logger = logger;
			_api    = api;
			_users  = users;
		}
		
		public ApiResponse Add(User user) {
			_logger.DebugFormat(this, "Add: {0}", user);
			if ( (user == null) || string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.PasswordHash) ) {
				return _api.WithError(new ClientError("Invalid user"));
			}
			if ( !_users.TryAdd(user) ) {
				return _api.WithError(new ServerError());
			}
			_logger.DebugFormat(this, "Add: successfully: {0}", user);
			return _api.Ok();
		}
	}
}