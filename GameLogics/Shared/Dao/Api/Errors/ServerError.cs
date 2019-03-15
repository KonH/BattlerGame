namespace GameLogics.Shared.Dao.Api.Errors {
	public class ServerError : BaseError {
		public ServerError(string message = "Internal error") : base(message) {}
	}
}