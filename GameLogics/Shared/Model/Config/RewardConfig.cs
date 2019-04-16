namespace GameLogics.Shared.Model.Config {
	public sealed class RewardConfig {
		public RewardInterval Resources { get; set; } = new RewardInterval();
		public RewardInterval Units     { get; set; } = new RewardInterval();
		public RewardInterval Items     { get; set; } = new RewardInterval();
	}
}