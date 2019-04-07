namespace GameLogics.Shared.Dao.Api.Errors {
	public sealed class ConflictError : BaseError {
		public ConflictError(string message) : base(message) {}
	}
}