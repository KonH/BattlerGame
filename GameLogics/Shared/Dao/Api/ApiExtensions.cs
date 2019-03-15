using GameLogics.Shared.Dao.Api.Errors;

namespace GameLogics.Shared.Dao.Api {
	public static class ApiExtensions {
		public static ApiResponse<T> AsError<T>(this IApiError error) {
			return new ApiResponse<T>(default(T), error);
		}

		public static ApiResponse<T> AsResult<T>(this T result) {
			return new ApiResponse<T>(result, null);
		}
	}
}