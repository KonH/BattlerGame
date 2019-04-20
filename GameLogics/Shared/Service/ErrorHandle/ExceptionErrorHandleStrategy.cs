using System;
using GameLogics.Shared.Dao.Api.Error;

namespace GameLogics.Shared.Service.ErrorHandle {
	public sealed class ExceptionErrorHandleStrategy : IErrorHandleStrategy {
		public void OnError(IApiError error) {
			throw new InvalidOperationException(error.Message);
		}
	}
}