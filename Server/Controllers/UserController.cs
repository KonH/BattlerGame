using Microsoft.AspNetCore.Mvc;
using GameLogics.Models;
using GameLogics.Server.Services;
using Server.Utils;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {
		readonly UserService _service;
		
		public UserController(UserService service) {
			_service = service;
		}

		[HttpPost]
		public IActionResult Add([FromBody] User user) {
			return this.Wrap(_service.Add(user));
		}
	}
}