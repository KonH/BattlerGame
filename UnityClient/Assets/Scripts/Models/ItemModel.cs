using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace UnityClient.Models {
	public class ItemModel {
		public ItemState  State    { get; }
		public ItemConfig Config   { get; }
		public ItemType   FakeType { get; }

		public bool IsFake => State == null;

		public ItemModel(ItemState state, ItemConfig config) {
			State  = state;
			Config = config;
		}

		public ItemModel(ItemType fakeType) {
			FakeType = fakeType;
		}
	}
}