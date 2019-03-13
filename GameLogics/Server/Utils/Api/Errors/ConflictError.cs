namespace GameLogics.Server.Utils.Api.Errors {
	public class ConflictError : BaseError {
		public ConflictError(string message = "conflict error") : base(message) {}
	}
}