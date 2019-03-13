using GameLogics.Models;
using GameLogics.Server.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Server.Utils;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller {
		readonly AuthService _service;

		public AuthController(AuthService service) {
			_service = service;
		}
		
		[HttpPost]
		public IActionResult RequestToken(User user) {
			return this.Wrap(_service.RequestToken(user));
		}
	}
}