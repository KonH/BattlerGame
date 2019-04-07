namespace GameLogics.Shared.Dao.Auth {
	public sealed class AuthRequest {
		public string Login        { get; set; }
		public string PasswordHash { get; set; }

		public AuthRequest(string login, string passwordHash) {
			Login        = login;
			PasswordHash = passwordHash;
		}

		public override string ToString() {
			return $"Login: '{Login}', PasswordHash: '{PasswordHash}'";
		}
	}
}