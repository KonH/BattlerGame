using System.IdentityModel.Tokens.Jwt;
using GameLogics.DAO;
using GameLogics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Repositories;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller {
		readonly ILogger         _logger;
		readonly IUserRepository _users;
		readonly AuthService     _service;

		public AuthController(ILogger<AuthController> logger, IUserRepository users, AuthService service) {
			_logger  = logger;
			_users   = users;
			_service = service;
		}
		
		[HttpPost]
		public IActionResult RequestToken(User user) {
			var storedUser = _users.Find(user.Login, user.PasswordHash);
			if ( storedUser == null ) {
				_logger.LogDebug("No user in repository or invalid password");
				return BadRequest("Invalid login or password");
			}
			var jwt = _service.CreateToken(storedUser);
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
			var response = new AuthResponse(encodedJwt, storedUser.Login);
			_logger.LogDebug("User is logged in: '{0}'", storedUser.Login);
			return Json(response);
		}
	}
}