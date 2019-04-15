using System.Threading.Tasks;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Service;

namespace Server.Controller {
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public sealed class IntentController : BaseApiController<IntentRequest, IntentResponse> {
		public IntentController(ActionResultWrapper wrapper, IApiService service) : base(wrapper, service.Post) {}

		[HttpPost]
		public override Task<IActionResult> Post(IntentRequest req) {
			if ( User.Identity.Name != req.Login ) {
				return Task.FromResult<IActionResult>(BadRequest("Intent from incorrect user"));
			}
			return base.Post(req);
		}
	}
}