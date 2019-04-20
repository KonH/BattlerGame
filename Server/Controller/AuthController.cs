using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Service;
using Microsoft.AspNetCore.Mvc;
using Server.Service;

namespace Server.Controller {
	[Route("api/[controller]")]
	[ApiController]
	public sealed class AuthController : BaseApiController<AuthRequest, AuthResponse> {
		public AuthController(ActionResultWrapper wrapper, IApiService service) : base(wrapper, service.Post) {}
	}
}