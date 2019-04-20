namespace GameLogics.Shared.Dao.Api.Error {
	public sealed class ConflictError : BaseError {
		public ConflictError(string message) : base(message) {}
	}
}