namespace GameLogics.Client.Utils {
	public sealed class NetworkResponse {
		public bool   Success    { get; }
		public string Text       { get; }
		public int    StatusCode { get; }

		public NetworkResponse(bool success, string text, int statusCode) {
			Success    = success;
			Text       = text;
			StatusCode = statusCode;
		}
	}
}