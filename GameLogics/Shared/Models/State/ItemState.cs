namespace GameLogics.Shared.Models.State {
	public sealed class ItemState {
		public ulong   Id         { get; set; }
		public string  Descriptor { get; set; }
		public int     Level      { get; set; }

		public ItemState(string descriptor) {
			Descriptor = descriptor;
		}

		public ItemState WithId(ulong id) {
			Id = id;
			return this;
		}
	}
}