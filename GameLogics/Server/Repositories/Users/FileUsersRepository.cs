using GameLogics.Server.Models;

namespace GameLogics.Server.Repositories.Users {
	public sealed class FileUsersRepository : IUsersRepository {
		FileStorageRepository _file;

		public FileUsersRepository(FileStorageRepository file) {
			_file = file;
		}
		
		public User Find(string login, string passwordHash = null) {
			if ( _file.State.Users.TryGetValue(login, out var user) ) {
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
			if ( _file.State.Users.ContainsKey(user.Login) ) {
				return false;
			}
			_file.State.Users.Add(user.Login, user);
			_file.Save();
			return true;
		}
	}
}