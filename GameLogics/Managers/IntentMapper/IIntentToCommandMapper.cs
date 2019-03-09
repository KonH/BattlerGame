using System.Threading.Tasks;
using GameLogics.Intents;

namespace GameLogics.Managers.IntentMapper {
	public interface IIntentToCommandMapper {
		Task<CommandResponse> RequestCommandsFromIntent(IIntent intent);
	}
}