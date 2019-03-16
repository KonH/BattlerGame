using GameLogics.Shared.Dao.Auth;
using GameLogics.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : BaseApiController<AuthRequest, AuthResponse> {
		public AuthController(ActionResultWrapper wrapper, IApiService service) : base(wrapper, service.Post) {}
	}
}