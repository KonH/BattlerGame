using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Managers;
using GameLogics.Managers.Network;

namespace ConsoleClient {
	public class HttpClientNetworkManager : INetworkManager {
		readonly HttpClient _client = new HttpClient();

		readonly ICustomLogger _logger;
		
		readonly string _baseUrl;
		
		public HttpClientNetworkManager(ICustomLogger logger, string baseUrl) {
			_logger  = logger;
			_baseUrl = baseUrl;
		}
		
		public async Task<NetworkResponse> PostJson(string relativeUrl, string body) {
			try {
				var content      = new StringContent(body, Encoding.UTF8, "application/json");
				var result       = await _client.PostAsync(_baseUrl + relativeUrl, content);
				var responseText = await result.Content.ReadAsStringAsync();
				return new NetworkResponse((int)result.StatusCode, result.IsSuccessStatusCode, responseText);
			} catch ( Exception e ) {
				_logger.ErrorFormat("PostJson: {0}", e);
				return new NetworkResponse(-1, false, "");
			}
		}
	}
}