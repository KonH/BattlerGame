namespace GameLogics.Server.Models {
	public sealed class User {
		public string Login        { get; }
		public string PasswordHash { get; }
		public string Name         { get; }
		public string Role         { get; }
		
		public User(string login, string passwordHash, string name, string role) {
			Login        = login;
			PasswordHash = passwordHash;
			Name         = name;
			Role         = role;
		}

		public override string ToString() {
			return $"{nameof(Login)}: {Login}, {nameof(PasswordHash)}: {PasswordHash}, {nameof(Name)}: {Name}, {nameof(Role)}: {Role}";
		}
		
		bool Equals(User other) {
			return string.Equals(Login, other.Login);
		}

		public override bool Equals(object obj) {
			if ( ReferenceEquals(null, obj) ) {
				return false;
			}
			if ( ReferenceEquals(this, obj) ) {
				return true;
			}
			return Equals((User)obj);
		}

		public override int GetHashCode() {
			return Login.GetHashCode();
		}

		public static bool operator ==(User left, User right) {
			return Equals(left, right);
		}

		public static bool operator !=(User left, User right) {
			return !Equals(left, right);
		}
	}
}