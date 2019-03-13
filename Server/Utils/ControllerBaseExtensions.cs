using System;
using GameLogics.Server.Utils.Api;
using GameLogics.Server.Utils.Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Server.Utils {
	public static class ControllerBaseExtensions {
		public static IActionResult Wrap(this ControllerBase self, ApiResponse response) {
			switch ( response.Error ) {
				case null           : return self.Ok(response.Text);
				case ClientError   e: return self.BadRequest(e.Message);
				case ConflictError e: return self.Conflict(e.Message);
				case ServerError   e: return self.StatusCode(500, e.Message);
				default             : throw new InvalidOperationException("Unexpected response type");
			}
		}
	}
}