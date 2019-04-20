namespace GameLogics.Server.Model {
	public sealed class UserState {
		public string Login        { get; }
		public string PasswordHash { get; }
		public string Name         { get; }
		public string Role         { get; }
		
		public UserState(string login, string passwordHash, string name, string role) {
			Login        = login;
			PasswordHash = passwordHash;
			Name         = name;
			Role         = role;
		}

		public override string ToString() {
			return $"{nameof(Login)}: {Login}, {nameof(PasswordHash)}: {PasswordHash}, {nameof(Name)}: {Name}, {nameof(Role)}: {Role}";
		}
		
		bool Equals(UserState other) {
			return string.Equals(Login, other.Login);
		}

		public override bool Equals(object obj) {
			if ( ReferenceEquals(null, obj) ) {
				return false;
			}
			if ( ReferenceEquals(this, obj) ) {
				return true;
			}
			return Equals((UserState)obj);
		}

		public override int GetHashCode() {
			return Login.GetHashCode();
		}

		public static bool operator ==(UserState left, UserState right) {
			return Equals(left, right);
		}

		public static bool operator !=(UserState left, UserState right) {
			return !Equals(left, right);
		}
	}
}