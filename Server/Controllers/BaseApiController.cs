using System;
using GameLogics.Shared.Dao.Api;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	public abstract class BaseApiController<TRequest, TResponse> : ControllerBase {
		readonly ActionResultWrapper                    _wrapper;
		readonly Func<TRequest, ApiResponse<TResponse>> _handler;
		
		public BaseApiController(ActionResultWrapper wrapper, Func<TRequest, ApiResponse<TResponse>> handler) {
			_wrapper = wrapper;
			_handler = handler;
		}
		
		[HttpPost]
		public virtual IActionResult Post(TRequest req) {
			return _wrapper.Wrap(_handler(req));
		}
	}
}