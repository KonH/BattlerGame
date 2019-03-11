using GameLogics.Models;

namespace Server.Repositories {
	public interface IUserRepository {
		User Find(string login, string passwordHash);
		bool TryAdd(User user);
	}
}