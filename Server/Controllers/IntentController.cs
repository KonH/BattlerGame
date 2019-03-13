using System.Threading.Tasks;
using GameLogics.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server.Controllers {
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class IntentController : ControllerBase {
		readonly ILogger       _logger;
		readonly IntentService _service;

		public IntentController(ILogger<IntentController> logger, IntentService service) {
			_logger  = logger;
			_service = service;
		}
		
		[HttpPost]
		public async Task<ActionResult<CommandResponse>> Post([FromBody] IntentRequest request) {
			_logger.LogDebug($"Incoming: {request}");
			var response = await _service.CreateResponse(User.Identity.Name, request.ExpectedVersion, request.Intent);
			if ( !response.Success ) {
				return BadRequest(response.Error.GetType().ToString());
			}
			_logger.LogDebug($"Outgoing: {response}");
			return new ActionResult<CommandResponse>(response);
		}
	}
}