using Microsoft.AspNetCore.Mvc;
using GameLogics.Shared.Dao.Register;
using GameLogics.Shared.Services;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public sealed class RegisterController : BaseApiController<RegisterRequest, RegisterResponse> {
		public RegisterController(ActionResultWrapper wrapper, IApiService service) : base(wrapper, service.Post) {}
	}
}