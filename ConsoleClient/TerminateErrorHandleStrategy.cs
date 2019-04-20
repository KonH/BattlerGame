using System;
using GameLogics.Shared.Service.ErrorHandle;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Service;

namespace ConsoleClient {
	public sealed class TerminateErrorHandleStrategy : IErrorHandleStrategy {
		readonly ICustomLogger _logger;
		
		public TerminateErrorHandleStrategy(ICustomLogger logger) {
			_logger = logger;
		}
		
		public void OnError(IApiError error) {
			_logger.Error(this, $"Failed: '{error.Message}' ({error.GetType().Name})");
			Environment.Exit(-1);
		}
	}
}