using GameLogics.Server.Model;

namespace GameLogics.Server.Repository.User {
	public interface IUserRepository {
		UserState Find(string login, string passwordHash = null);
		bool TryAdd(UserState user);
	}
}