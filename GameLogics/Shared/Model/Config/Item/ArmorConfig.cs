namespace GameLogics.Shared.Model.Config {
	public sealed class ArmorConfig : BaseItemConfig {
		public override ItemType Type => ItemType.Armor;
		
		public int[] Absorb { get; set; }

		public ArmorConfig() {}
		
		public ArmorConfig(int absorb) {
			Absorb = new int[] { absorb };
		}

		public int GetAbsorbForLevel(int level) => Absorb[level];
	}
}