using System;
using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.Managers.IntentMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Server.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class IntentController : ControllerBase {
		readonly ILogger                     _logger;
		readonly DirectIntentToCommandMapper _mapper;

		public IntentController(ILoggerFactory loggerFactory, DirectIntentToCommandMapper mapper) {
			_logger = loggerFactory.CreateLogger<IntentController>();
			_mapper = mapper;
		}
		
		[HttpPost]
		public async Task<ActionResult<IntentResponse>> Post([FromBody] IntentRequest request) {
			_logger.LogDebug($"Incoming: {request}");
			var response = await _mapper.RequestCommandsFromIntent(request.Intent);
			if ( !response.Success ) {
				throw new InvalidOperationException("internal error");
			}
			var commands = response.Commands;
			var responseDto = new IntentResponse { Commands = commands };
			_logger.LogDebug($"Outgoing: {responseDto}");
			return new ActionResult<IntentResponse>(responseDto);
		}
	}
}