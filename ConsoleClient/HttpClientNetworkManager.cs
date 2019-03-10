using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameLogics.Managers.Network;

namespace ConsoleClient {
	public class HttpClientNetworkManager : INetworkManager {
		readonly HttpClient _client = new HttpClient();
		
		readonly string _baseUrl;
		
		public HttpClientNetworkManager(string baseUrl) {
			_baseUrl = baseUrl;
		}
		
		public async Task<NetworkResponse> PostJson(string relativeUrl, string body) {
			var content = new StringContent(body, Encoding.UTF8, "application/json");
			try {
				var result = await _client.PostAsync(_baseUrl + relativeUrl, content);
				var responseText = await result.Content.ReadAsStringAsync();
				return new NetworkResponse((int)result.StatusCode, result.IsSuccessStatusCode, responseText);
			} catch ( Exception e ) {
				return new NetworkResponse(-1, false, e.ToString());
			}
		}
	}
}