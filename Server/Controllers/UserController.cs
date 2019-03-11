using Microsoft.AspNetCore.Mvc;
using GameLogics.Models;
using Microsoft.Extensions.Logging;
using Server.Repositories;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase {
		readonly ILogger         _logger;
		readonly IUserRepository _users;
		
		public UserController(ILogger<UserController> logger, IUserRepository users) {
			_logger = logger;
			_users  = users;
		}

		[HttpPost]
		public IActionResult Add([FromBody] User user) {
			_logger.LogDebug("Add: {0}", user.ToString());
			if ( _users.TryAdd(user) ) {
				return Ok();
			}
			return BadRequest();
		}
	}
}