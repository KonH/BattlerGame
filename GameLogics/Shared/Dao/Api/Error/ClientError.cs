namespace GameLogics.Shared.Dao.Api.Error {
	public sealed class ClientError : BaseError {
		public ClientError(string message) : base(message) {}
	}
}