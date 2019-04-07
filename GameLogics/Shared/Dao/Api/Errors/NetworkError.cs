namespace GameLogics.Shared.Dao.Api.Errors {
	public sealed class NetworkError : BaseError {
		public NetworkError(string message = "Network error") : base(message) {}
	}
}