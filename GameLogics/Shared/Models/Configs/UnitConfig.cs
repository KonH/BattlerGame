namespace GameLogics.Shared.Models.Configs {
	public sealed class UnitConfig {
		public int BaseDamage { get; set; }
		public int MaxHealth  { get; set; }

		public UnitConfig(int baseDamage, int maxHealth) {
			BaseDamage = baseDamage;
			MaxHealth  = maxHealth;
		}
	}
}