namespace GameLogics.Shared.Dao.Api.Error {
	public sealed class ServerError : BaseError {
		public ServerError(string message = "Internal error") : base(message) {}
	}
}