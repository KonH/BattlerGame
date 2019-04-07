namespace GameLogics.Shared.Dao.Api.Errors {
	public sealed class ClientError : BaseError {
		public ClientError(string message) : base(message) {}
	}
}