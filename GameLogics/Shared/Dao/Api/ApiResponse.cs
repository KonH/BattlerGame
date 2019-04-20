using GameLogics.Shared.Dao.Api.Error;

namespace GameLogics.Shared.Dao.Api {
	public class ApiResponse<T> {
		public T         Result  { get; }
		public IApiError Error   { get; }

		public bool Success => (Error == null);

		public ApiResponse(T result, IApiError error) {
			Result = result;
			Error  = error;
		}
	}
}