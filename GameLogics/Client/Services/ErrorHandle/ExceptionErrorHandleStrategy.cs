using System;
using GameLogics.Shared.Dao.Api.Errors;

namespace GameLogics.Client.Services.ErrorHandle {
	public class ExceptionErrorHandleStrategy : IErrorHandleStrategy {
		public void OnError(IApiError error) {
			throw new InvalidOperationException(error.Message);
		}
	}
}