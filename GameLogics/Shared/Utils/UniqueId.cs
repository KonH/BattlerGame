using System;

namespace GameLogics.Shared.Utils {
	public static class UniqueId {
		public static string New() {
			return Guid.NewGuid().ToString();
		}
	}
}