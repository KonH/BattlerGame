using GameLogics.Server.Services;
using GameLogics.Shared.Dao.Auth;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : BaseApiController<AuthRequest, AuthResponse> {
		public AuthController(ActionResultWrapper wrapper, AuthService service) : base(wrapper, service.RequestToken) {}
	}
}