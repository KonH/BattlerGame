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
		public async Task<ActionResult<IntentResponse>> Post([FromBody] IntentRequest request) {
			_logger.LogDebug($"Incoming: {request}");
			var response = await _service.CreateCommands(User.Identity.Name, request.Intent);
			if ( !response.Success ) {
				return BadRequest("internal error");
			}
			var commands = response.Commands;
			var responseDto = new IntentResponse { Commands = commands };
			_logger.LogDebug($"Outgoing: {responseDto}");
			return new ActionResult<IntentResponse>(responseDto);
		}
	}
}