using System.Threading.Tasks;
using GameLogics.DAO;
using GameLogics.Intents;
using GameLogics.Models;

namespace GameLogics.Managers.IntentMapper {
	public interface IIntentToCommandMapper {
		Task<CommandResponse> RequestCommandsFromIntent(GameState state, IIntent intent);
	}
}