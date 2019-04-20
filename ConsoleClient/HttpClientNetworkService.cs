using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Client.Service;
using GameLogics.Client.Utils;
using GameLogics.Shared.Service;

namespace ConsoleClient {
	public sealed class HttpClientNetworkService : INetworkService {
		readonly HttpClient _client = new HttpClient();

		readonly ICustomLogger _logger;
		
		readonly string _baseUrl;
		
		public string AuthToken { get; set; }
		
		public HttpClientNetworkService(ICustomLogger logger, string baseUrl) {
			_logger  = logger;
			_baseUrl = baseUrl;
		}
		
		public async Task<NetworkResponse> PostJson(string relativeUrl, string body) {
			try {
				if ( !string.IsNullOrEmpty(AuthToken) ) {
					_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
				}
				var content      = new StringContent(body, Encoding.UTF8, "application/json");
				var result       = await _client.PostAsync(_baseUrl + relativeUrl, content);
				var responseText = await result.Content.ReadAsStringAsync();
				if ( !result.IsSuccessStatusCode ) {
					_logger.ErrorFormat(this, "PostJson failed: {0} {1}", (int)result.StatusCode, result.ReasonPhrase);
				}
				return new NetworkResponse(result.IsSuccessStatusCode, responseText, (int)result.StatusCode);
			} catch ( Exception e ) {
				_logger.ErrorFormat(this, "PostJson failed: {0}", e);
				return new NetworkResponse(false, "", -1);
			}
		}
	}
}