namespace GameLogics.Shared.Model.Config {
	public sealed class RewardConfig {
		public RewardInterval Resources { get; } = new RewardInterval();
		public RewardInterval Units     { get; } = new RewardInterval();
		public RewardInterval Items     { get; } = new RewardInterval();
	}
}