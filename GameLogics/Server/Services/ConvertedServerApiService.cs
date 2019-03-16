using System;
using System.Threading.Tasks;
using GameLogics.Client.Services.ErrorHandle;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	/// <summary>
	/// For emulation purposes to prevent modifications of same objects
	/// </summary>
	public class ConvertedServerApiService : ServerApiService {
		readonly ConvertService       _convert;
		readonly IErrorHandleStrategy _errorHandle;

		public ConvertedServerApiService(
			ConvertService convert, ICustomLogger logger, IErrorHandleStrategy errorHandle,
			RegisterService register, AuthService auth, IntentService intent) :
			base(logger, register, auth, intent) {
			_convert     = convert;
			_errorHandle = errorHandle;
		}

		protected override Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(TRequest req, Func<TRequest, ApiResponse<TResponse>> handler) {
			var task = base.Post(DoubleConvert(req), handler);
			var resp = DoubleConvert(task.Result);
			if ( !resp.Success ) {
				_errorHandle.OnError(resp.Error);
			}
			return Task.FromResult(resp);
		}

		T DoubleConvert<T>(T obj) {
			var json = _convert.ToJson(obj);
			var newObj = _convert.FromJson<T>(json);
			return newObj;
		}
	}
}