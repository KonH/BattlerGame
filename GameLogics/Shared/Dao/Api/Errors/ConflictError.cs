namespace GameLogics.Shared.Dao.Api.Errors {
	public class ConflictError : BaseError {
		public ConflictError(string message) : base(message) {}
	}
}