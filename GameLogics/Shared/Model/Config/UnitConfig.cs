namespace GameLogics.Shared.Model.Config {
	public sealed class UnitConfig {
		public int[] BaseDamage { get; set; }
		public int[] MaxHealth  { get; set; }
		public int   Experience { get; set; }

		public UnitConfig() {}

		public UnitConfig(int baseDamage, int maxHealth, int experience = 0) {
			BaseDamage = new int[] { baseDamage };
			MaxHealth  = new int[] { maxHealth };
			Experience = experience;
		}
	}
}