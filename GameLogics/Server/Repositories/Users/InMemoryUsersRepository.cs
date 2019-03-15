using System.Collections.Concurrent;
using GameLogics.Client.Utils;
using GameLogics.Server.Models;

namespace GameLogics.Server.Repositories.Users {
	public class InMemoryUsersRepository : IUsersRepository {
		ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User> {
			["test"] = new User("test", HashUtils.MakePasswordHash("test", "test"), "test", "user")
		};
		
		public User Find(string login, string passwordHash = null) {
			if ( _users.TryGetValue(login, out var user) ) {
				if ( (passwordHash == null) || (user.PasswordHash == passwordHash) ) {
					return user;
				}
			}
			return null;
		}

		public bool TryAdd(User user) {
			if ( (user == null) || (string.IsNullOrEmpty(user.Login)) ) {
				return false;
			}
			if ( _users.ContainsKey(user.Login) ) {
				return false;
			}
			return _users.TryAdd(user.Login, user);
		}
	}
}