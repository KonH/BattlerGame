namespace GameLogics.Shared.Model.Config {
	public sealed class WeaponConfig : BaseItemConfig {
		public override ItemType Type => ItemType.Weapon;
		
		public int[] Damage { get; set; }

		public WeaponConfig() {}
		
		public WeaponConfig(int damage) {
			Damage = new int[] { damage };
		}

		public int GetDamageForLevel(int level) => Damage[level];
	}
}