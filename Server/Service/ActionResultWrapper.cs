using System;
using GameLogics.Shared.Dao.Api;
using GameLogics.Shared.Dao.Api.Error;
using GameLogics.Shared.Service;
using Microsoft.AspNetCore.Mvc;

namespace Server.Service {
	public sealed class ActionResultWrapper {
		readonly ConvertService _convert;

		public ActionResultWrapper(ConvertService convert) {
			_convert = convert;
		}
		
		public IActionResult Wrap<T>(ApiResponse<T> response) {
			switch ( response.Error ) {
				case null           : return new ObjectResult(_convert.ToJson(response.Result));
				case ClientError   e: return new BadRequestObjectResult(e.Message);
				case ConflictError e: return new ConflictObjectResult(e.Message);
				case ServerError   e: return new ObjectResult(e.Message) { StatusCode = 500 };
				default             : throw new InvalidOperationException("Unexpected response type");
			}
		}
	}
}