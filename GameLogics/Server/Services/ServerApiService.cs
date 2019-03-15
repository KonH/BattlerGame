using System;
using System.Threading.Tasks;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Dao.Register;
using GameLogics.Shared.Services;

namespace GameLogics.Server.Services {
	public class ServerApiService : IApiService {
		readonly ICustomLogger   _logger;
		readonly RegisterService _register;
		readonly AuthService     _auth;
		readonly IntentService   _intent;

		public ServerApiService(ICustomLogger logger, RegisterService register, AuthService auth, IntentService intent) {
			_logger   = logger;
			_register = register;
			_auth     = auth;
			_intent   = intent;
		}

		public Task<ApiResponse<RegisterResponse>> Post(RegisterRequest req) => Post(req, _register.RegisterNewUser);
		public Task<ApiResponse<AuthResponse>> Post(AuthRequest req) => Post(req, _auth.RequestToken);
		public Task<ApiResponse<IntentResponse>> Post(IntentRequest req) => Post(req, _intent.CreateCommands);

		protected virtual Task<ApiResponse<TResponse>> Post<TRequest, TResponse>(TRequest req, Func<TRequest, ApiResponse<TResponse>> handler) {
			_logger.Debug(this, $"Request ({typeof(TRequest).Name}): '{req.ToString()}'");
			var resp = handler(req);
			_logger.Debug(this, $"Response ({typeof(TResponse).Name}): '{resp.Success}, '{resp.Result}', {resp.Error}");
			return Task.FromResult(resp);
		}
	}
}