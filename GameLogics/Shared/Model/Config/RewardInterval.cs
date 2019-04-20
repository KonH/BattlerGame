namespace GameLogics.Shared.Model.Config {
	public sealed class RewardInterval {
		public int Min { get; set; }
		public int Max { get; set; }

		public RewardInterval() {}

		public RewardInterval(int min, int max) {
			Min = min;
			Max = max;
		}
	}
}