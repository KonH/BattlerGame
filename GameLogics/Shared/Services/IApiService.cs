using System;
using System.Threading.Tasks;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Dao.Register;

namespace GameLogics.Shared.Services {
	public interface IApiService {
		event Action<IApiError> OnError;
		Task<ApiResponse<RegisterResponse>> Post(RegisterRequest req);
		Task<ApiResponse<AuthResponse>> Post(AuthRequest req);
		Task<ApiResponse<IntentResponse>> Post(IntentRequest req);
	}
}