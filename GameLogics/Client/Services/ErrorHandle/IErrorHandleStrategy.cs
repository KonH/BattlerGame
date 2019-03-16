using GameLogics.Shared.Dao.Api.Errors;

namespace GameLogics.Client.Services.ErrorHandle {
	public interface IErrorHandleStrategy {
		void OnError(IApiError error);
	}
}