namespace GameLogics.Shared.Models.State {
	public class ItemState {
		public ulong   Id         { get; set; }
		public string  Descriptor { get; set; }

		public ItemState(string descriptor) {
			Descriptor = descriptor;
		}

		public ItemState WithId(ulong id) {
			Id = id;
			return this;
		}
	}
}