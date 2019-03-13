using System.IdentityModel.Tokens.Jwt;
using GameLogics.DAO;
using GameLogics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Repositories.States;
using Server.Repositories.Users;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller {
		readonly ILogger              _logger;
		readonly IUserRepository      _users;
		readonly IGameStateRepository _states;
		readonly AuthService          _service;

		public AuthController(ILogger<AuthController> logger, IUserRepository users, IGameStateRepository states, AuthService service) {
			_logger  = logger;
			_users   = users;
			_states  = states;
			_service = service;
		}
		
		[HttpPost]
		public IActionResult RequestToken(User user) {
			var storedUser = _users.Find(user.Login, user.PasswordHash);
			if ( storedUser == null ) {
				if ( _users.Find(user.Login) == null ) {
					_logger.LogDebug($"No user in repository with login '{user.Login}'");
				} else {
					_logger.LogDebug($"Invalid password for user with login '{user.Login}'");
				}
				return BadRequest("Invalid login or password");
			}
			var jwt = _service.CreateToken(storedUser);
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
			var state = _states.FindOrCreate(storedUser);
			var response = new AuthResponse(encodedJwt, storedUser.Login, state, state.Version);
			_logger.LogDebug("User is logged in: '{0}'", storedUser.Login);
			return Json(response);
		}
	}
}