using GameLogics.Shared.Command.Base;
using GameLogics.Shared.Logic;
using GameLogics.Shared.Model.Config;
using GameLogics.Shared.Model.State;

namespace GameLogics.Shared.Command {
	public sealed class ClaimEventRewardCommand : ICommand {
		public readonly string EventName;

		public ClaimEventRewardCommand(string eventName) {
			EventName = eventName;
		}

		public bool IsValid(GameState state, ConfigRoot config) { 
			return !state.Events.ContainsKey(EventName) && EventLogic.IsCompleted(EventName, state, config);
		}

		public void Execute(GameState state, ConfigRoot config, ICommandBuffer buffer) {
			var info = config.Events[EventName];
			state.Events[EventName] = true;
			state.Progress.Remove(info.Scope);
			var rewardLevel = info.RewardLevel;
			RewardLogic.AppendReward(rewardLevel, state, config, buffer);
		}

		public override string ToString() {
			return $"{nameof(ClaimEventRewardCommand)} ('{EventName}')";
		}
	}
}
