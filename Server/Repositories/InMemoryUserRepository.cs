using System.Collections.Concurrent;
using GameLogics.Models;

namespace Server.Repositories {
	public class InMemoryUserRepository : IUserRepository {
		ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User> {
			["test"] = User.CreateWithPassword("test", "test", "test", "user")
		};
		
		public User Find(string login, string passwordHash) {
			if ( _users.TryGetValue(login, out var user) ) {
				if ( user.PasswordHash == passwordHash ) {
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