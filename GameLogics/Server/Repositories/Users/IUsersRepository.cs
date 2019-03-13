using GameLogics.Models;

namespace GameLogics.Server.Repositories.Users {
	public interface IUsersRepository {
		User Find(string login, string passwordHash = null);
		bool TryAdd(User user);
	}
}