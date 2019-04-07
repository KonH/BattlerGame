using System;
using System.Threading.Tasks;
using GameLogics.Client.Services.ErrorHandle;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	/// <summary>
	/// For emulation purposes to prevent modifications of same objects
	/// </summary>
	public sealed class ConvertedServerApiService : ServerApiService {
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
			try {
				var task = base.Post(_convert.DoubleConvert(req), handler);
				var resp = _convert.DoubleConvert(task.Result);
				if ( !resp.Success ) {
					_errorHandle.OnError(resp.Error);
				}
				return Task.FromResult(resp);
			} catch ( Exception e ) {
				_logger.Error(this, $"Post: {e}");
				return Task.FromResult(new ServerError(e.ToString()).AsError<TResponse>());
			}
		}
	}
}