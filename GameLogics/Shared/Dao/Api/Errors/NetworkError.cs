namespace GameLogics.Shared.Dao.Api.Errors {
	public class NetworkError : BaseError {
		public NetworkError(string message = "Network error") : base(message) {}
	}
}