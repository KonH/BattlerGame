using GameLogics.Shared.Dao.Api.Errors;

namespace GameLogics.Client.Services.ErrorHandle {
	public sealed class EmptyErrorHandleStrategy : IErrorHandleStrategy {
		public void OnError(IApiError error) {}
	}
}