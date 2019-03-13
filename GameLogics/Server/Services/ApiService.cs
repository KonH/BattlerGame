using GameLogics.Managers;
using GameLogics.Server.Utils.Api;
using GameLogics.Server.Utils.Api.Errors;
using Newtonsoft.Json;

namespace GameLogics.Server.Services {
	public class ApiService {
		readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
			TypeNameHandling = TypeNameHandling.Auto
		};

		readonly ICustomLogger _logger;

		public ApiService(ICustomLogger logger) {
			_logger = logger;
		}

		public ApiResponse Ok(string text = "") {
			return new ApiResponse(text, null);
		}

		public ApiResponse Json(object obj) {
			var json = JsonConvert.SerializeObject(obj, _settings);
			_logger.Debug(this, $"Json: '{json}'");
			return Ok(json);
		}
		
		public ApiResponse WithError(IApiError error) {
			_logger.Error(this, $"WithError: '{error.Message}' ({error.GetType().Name})");
			return new ApiResponse(string.Empty, error);
		}
	}
}