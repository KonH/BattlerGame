namespace GameLogics.Managers.Network {
	public class NetworkResponse {
		public int    StatusCode   { get; }
		public bool   IsSuccess    { get; }
		public string ResponseText { get; }

		public NetworkResponse(int statusCode, bool isSuccess, string responseText) {
			StatusCode   = statusCode;
			IsSuccess    = isSuccess;
			ResponseText = responseText;
		}

		public override string ToString() {
			return $"StatusCode: {StatusCode.ToString()}, IsSuccess: {IsSuccess.ToString()}, ResponseText: '{ResponseText}'";
		}
	}
}