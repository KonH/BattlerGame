namespace GameLogics.Shared.Models.Configs {
	public class WeaponConfig : IItemConfig {
		public ItemType Type => ItemType.Weapon;
		
		public int Damage { get; set; }

		public WeaponConfig() {}
		
		public WeaponConfig(int damage) {
			Damage = damage;
		}
	}
}