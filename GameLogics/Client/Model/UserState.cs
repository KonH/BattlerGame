using GameLogics.Client.Utils;

namespace GameLogics.Client.Model {
	public sealed class UserState {
		public string Login    { get; }
		public string Password { get; }
		public string Name     { get; }
		public string Token    { get; set; }

		public string PasswordHash => HashUtils.MakePasswordHash(Login, Password);

		public UserState(string login, string password, string name) {
			Login    = login;
			Password = password;
			Name     = name;
		}
	}
}