using GameLogics.Shared.Utils;

namespace GameLogics.Shared.Models {
	public class ItemState {
		public string   Id         { get; set; }
		public string   Descriptor { get; set; }

		public ItemState(string descriptor) {
			Descriptor = descriptor;
		}

		public ItemState WithId(string id) {
			Id = id;
			return this;
		}

		public ItemState WithNewId() => WithId(UniqueId.New());
	}
}