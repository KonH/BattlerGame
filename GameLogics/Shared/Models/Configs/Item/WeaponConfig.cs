namespace GameLogics.Shared.Models.Configs {
	public sealed class WeaponConfig : IItemConfig {
		public ItemType Type => ItemType.Weapon;
		
		public int[] Damage { get; set; }

		public WeaponConfig() {}
		
		public WeaponConfig(int damage) {
			Damage = new int[] { damage };
		}

		public int GetDamageForLevel(int level) => Damage[level];
	}
}