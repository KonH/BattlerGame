namespace GameLogics.Shared.Dao.Register {
	public sealed class RegisterRequest {
		public string Name         { get; set; }
		public string Login        { get; set; }
		public string PasswordHash { get; set; }

		public RegisterRequest(string name, string login, string passwordHash) {
			Name         = name;
			Login        = login;
			PasswordHash = passwordHash;
		}

		public override string ToString() {
			return $"Name: '{Name}', Login: '{Login}', PasswordHash: '{PasswordHash}'";
		}
	}
}