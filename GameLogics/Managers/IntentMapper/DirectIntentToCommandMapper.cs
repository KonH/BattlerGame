using System.Threading.Tasks;
using GameLogics.Intents;

namespace GameLogics.Managers.IntentMapper {
	public class DirectIntentToCommandMapper : BaseIntentToCommandMapper {		
		public DirectIntentToCommandMapper(IGameStateManager stateManager): base(stateManager) {}

		public override Task<CommandResponse> RequestCommandsFromIntent(IIntent intent) {
			return Task.FromResult(CommandResponse.FromCommands(CreateCommandsFromIntent(intent)));
		}
	}
}