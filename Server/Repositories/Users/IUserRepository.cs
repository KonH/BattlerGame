using GameLogics.Models;

namespace Server.Repositories.Users {
	public interface IUserRepository {
		User Find(string login, string passwordHash = null);
		bool TryAdd(User user);
	}
}