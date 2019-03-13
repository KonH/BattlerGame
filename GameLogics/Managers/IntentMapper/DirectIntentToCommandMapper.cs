using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.Intents;
using GameLogics.Models;

namespace GameLogics.Managers.IntentMapper {
	public class DirectIntentToCommandMapper : BaseIntentToCommandMapper {
		public override Task<CommandResponse> RequestCommandsFromIntent(GameState state, IIntent intent) {
			return Task.FromResult(CommandResponse.FromCommands(string.Empty, CreateCommandsFromIntent(state, intent)));
		}
	}
}