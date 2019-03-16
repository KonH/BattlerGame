using System;
using System.Threading.Tasks;
using GameLogics.Shared.Dao.Api;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	public abstract class BaseApiController<TRequest, TResponse> : ControllerBase {
		readonly ActionResultWrapper                          _wrapper;
		readonly Func<TRequest, Task<ApiResponse<TResponse>>> _handler;
		
		public BaseApiController(ActionResultWrapper wrapper, Func<TRequest, Task<ApiResponse<TResponse>>> handler) {
			_wrapper = wrapper;
			_handler = handler;
		}
		
		[HttpPost]
		public virtual async Task<IActionResult> Post(TRequest req) {
			var result = await _handler(req);
			return _wrapper.Wrap(result);
		}
	}
}