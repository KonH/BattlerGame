using System.Threading.Tasks;
using GameLogics.Client.Utils;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Dao.Register;
using GameLogics.Shared.Services;
using GameLogics.Shared.Utils;
using Newtonsoft.Json;

namespace GameLogics.Client.Services {
	public class ClientApiService : IApiService {
		readonly ICustomLogger   _logger;
		readonly ConvertService  _converter;
		readonly INetworkService _network;

		public ClientApiService(ICustomLogger logger, ConvertService converter, INetworkService network) {
			_logger    = logger;
			_converter = converter;
			_network   = network;
		}

		public Task<ApiResponse<RegisterResponse>> Post(RegisterRequest req) => PostJson<RegisterResponse>("api/register", req);
		public Task<ApiResponse<AuthResponse>> Post(AuthRequest req) => PostJson<AuthResponse>("api/auth", req);
		public Task<ApiResponse<IntentResponse>> Post(IntentRequest req) => PostJson<IntentResponse>("api/intent", req);

		async Task<ApiResponse<TResponse>> PostJson<TResponse>(string relativeUrl, object req) {
			var jsonReq = _converter.ToJson(req);
			_logger.Debug(this, $"Request ({req.GetType().Name}) to '{relativeUrl}': '{jsonReq}'");
			var netResponse = await _network.PostJson(relativeUrl, jsonReq);
			ApiResponse<TResponse> result;
			if ( netResponse.Success ) {
				var response = _converter.FromJson<TResponse>(netResponse.Text);
				result = response.AsResult();
			} else {
				result = ConvertToError(netResponse).AsError<TResponse>();
			}
			_logger.Debug(this, $"Response ({typeof(TResponse).Name}) from '{relativeUrl}': {result.Success}, '{result.Result}', {result.Error}");
			return result;
		}

		IApiError ConvertToError(NetworkResponse response) {
			switch ( response.StatusCode ) {
				case 409:                            return new ConflictError(response.Text);
				case var x when x.Between(400, 499): return new ClientError(response.Text);
				case var x when x.Between(500, 499): return new ServerError();
				default:                             return new NetworkError();
			}
		}
	}
}