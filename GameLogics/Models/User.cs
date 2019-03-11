using GameLogics.Utils;

namespace GameLogics.Models {
	public class User {
		public string Login        { get; set; }
		public string PasswordHash { get; set; }
		public string Name         { get; set; }
		public string Role         { get; set; }
		
		public User(string login, string passwordHash, string name, string role) {
			Login        = login;
			PasswordHash = passwordHash;
			Name         = name;
			Role         = role;
		}

		public override string ToString() {
			return $"{nameof(Login)}: {Login}, {nameof(PasswordHash)}: {PasswordHash}, {nameof(Name)}: {Name}, {nameof(Role)}: {Role}";
		}
		
		public static User CreateWithPassword(string login, string password, string name, string role) {
			return new User(login, HashUtils.MakePasswordHash(login, password), name, role);
		}
	}
}