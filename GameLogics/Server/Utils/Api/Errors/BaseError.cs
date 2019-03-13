namespace GameLogics.Server.Utils.Api.Errors {
	public class BaseError : IApiError {
		public string Message { get; }

		public BaseError(string message) {
			Message = message;
		}
	}
}