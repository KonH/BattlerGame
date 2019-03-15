using Microsoft.AspNetCore.Mvc;
using GameLogics.Server.Services;
using GameLogics.Shared.Dao.Register;
using Server.Services;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class RegisterController : BaseApiController<RegisterRequest, RegisterResponse> {
		public RegisterController(ActionResultWrapper wrapper, RegisterService service) : base(wrapper, service.RegisterNewUser) {}
	}
}