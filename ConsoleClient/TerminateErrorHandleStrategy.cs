using System;
using GameLogics.Client.Services.ErrorHandle;
using GameLogics.Shared.Dao.Api.Errors;
using GameLogics.Shared.Services;

namespace ConsoleClient {
	public class TerminateErrorHandleStrategy : IErrorHandleStrategy {
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