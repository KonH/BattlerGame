using GameLogics.Server.Model;

namespace GameLogics.Server.Repository.User {
	public sealed class FileUserRepository : IUserRepository {
		FileStorageRepository _file;

		public FileUserRepository(FileStorageRepository file) {
			_file = file;
		}
		
		public UserState Find(string login, string passwordHash = null) {
			if ( _file.State.Users.TryGetValue(login, out var user) ) {
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
			if ( _file.State.Users.ContainsKey(user.Login) ) {
				return false;
			}
			_file.State.Users.Add(user.Login, user);
			_file.Save();
			return true;
		}
	}
}