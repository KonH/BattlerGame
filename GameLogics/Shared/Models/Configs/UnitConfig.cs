namespace GameLogics.Shared.Models.Configs {
	public class UnitConfig {
		public int BaseDamage { get; set; }
		public int MaxHealth  { get; set; }

		public UnitConfig(int baseDamage, int maxHealth) {
			BaseDamage = baseDamage;
			MaxHealth  = maxHealth;
		}
	}
}