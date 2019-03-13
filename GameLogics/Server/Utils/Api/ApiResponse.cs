using GameLogics.Server.Utils.Api.Errors;

namespace GameLogics.Server.Utils.Api {
	public class ApiResponse {
		public string    Text  { get; }
		public IApiError Error { get; }

		public ApiResponse(string text, IApiError error) {
			Text  = text;
			Error = error;
		}
	}
}