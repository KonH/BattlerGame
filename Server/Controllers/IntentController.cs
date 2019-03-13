using System.Threading.Tasks;
using GameLogics.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Utils;

namespace Server.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class IntentController : ControllerBase {
		readonly IntentService _service;

		public IntentController(IntentService service) {
			_service = service;
		}
		
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] IntentRequest request) {
			var result = await _service.CreateResponse(User.Identity.Name, request.ExpectedVersion, request.Intent);
			return this.Wrap(result);
		}
	}
}