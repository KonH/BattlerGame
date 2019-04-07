namespace GameLogics.Shared.Dao.Api.Errors {
	public sealed class ServerError : BaseError {
		public ServerError(string message = "Internal error") : base(message) {}
	}
}