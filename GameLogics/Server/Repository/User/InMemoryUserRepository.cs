using System.Collections.Concurrent;
using GameLogics.Server.Model;

namespace GameLogics.Server.Repository.User {
	public sealed class InMemoryUserRepository : IUserRepository {
		ConcurrentDictionary<string, UserState> _users = new ConcurrentDictionary<string, UserState>();
		
		public UserState Find(string login, string passwordHash = null) {
			if ( _users.TryGetValue(login, out var user) ) {
				if ( (passwordHash == null) || (user.PasswordHash == passwordHash) ) {
					return user;
				}
			}
			return null;
		}

		public bool TryAdd(UserState user) {
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