namespace GameLogics.DAO {
	public class AuthResponse {
		public string Token    { get; set; }
		public string UserName { get; set; }

		public AuthResponse(string token, string userName) {
			Token    = token;
			UserName = userName;
		}
		
		public override string ToString() {
			return $"{nameof(Token)}: {Token}, {nameof(UserName)}: {UserName}";
		}
	}
}