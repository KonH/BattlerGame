namespace GameLogics.Shared.Models.Configs {
	public class RewardConfig {
		public RewardInterval Resources { get; } = new RewardInterval();
		public RewardInterval Units     { get; } = new RewardInterval();
		public RewardInterval Items     { get; } = new RewardInterval();
	}
}