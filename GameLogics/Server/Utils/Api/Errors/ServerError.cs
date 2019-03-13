namespace GameLogics.Server.Utils.Api.Errors {
	public class ServerError : BaseError {
		public ServerError(string message = "Internal error") : base(message) {}
	}
}