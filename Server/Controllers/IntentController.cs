using System.Threading.Tasks;
using GameLogics.Shared.Dao.Intent;
using GameLogics.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class IntentController : BaseApiController<IntentRequest, IntentResponse> {
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