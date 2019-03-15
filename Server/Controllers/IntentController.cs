using GameLogics.Server.Services;
using GameLogics.Shared.Dao.Intent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class IntentController : BaseApiController<IntentRequest, IntentResponse> {
		public IntentController(ActionResultWrapper wrapper, IntentService service) : base(wrapper, service.CreateCommands) {}

		[HttpPost]
		public override IActionResult Post(IntentRequest req) {
			if ( User.Identity.Name != req.Login ) {
				return BadRequest("Intent from incorrect user");
			}
			return base.Post(req);
		}
	}
}