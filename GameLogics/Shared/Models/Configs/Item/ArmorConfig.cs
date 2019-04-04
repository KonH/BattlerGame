namespace GameLogics.Shared.Models.Configs {
	public class ArmorConfig : IItemConfig {
		public ItemType Type => ItemType.Armor;
		
		public int Absorb { get; set; }

		public ArmorConfig() {}
		
		public ArmorConfig(int absorb) {
			Absorb = absorb;
		}
	}
}