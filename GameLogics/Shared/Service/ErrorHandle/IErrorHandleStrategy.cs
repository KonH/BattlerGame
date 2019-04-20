using GameLogics.Shared.Dao.Api.Error;

namespace GameLogics.Shared.Service.ErrorHandle {
	public interface IErrorHandleStrategy {
		void OnError(IApiError error);
	}
}