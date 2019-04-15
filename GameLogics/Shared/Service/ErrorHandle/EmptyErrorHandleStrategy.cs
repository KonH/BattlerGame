using GameLogics.Shared.Dao.Api.Error;

namespace GameLogics.Shared.Service.ErrorHandle {
	public sealed class EmptyErrorHandleStrategy : IErrorHandleStrategy {
		public void OnError(IApiError error) {}
	}
}