namespace GameLogics.Shared.Dao.Api.Error {
	public sealed class NetworkError : BaseError {
		public NetworkError(string message = "Network error") : base(message) {}
	}
}