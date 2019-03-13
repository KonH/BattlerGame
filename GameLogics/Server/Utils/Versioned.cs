using System;

namespace GameLogics.Server.Utils {
	public class Versioned<T> {
		public T      Value   { get; }
		public string Version { get; private set; }
		
		public Versioned(T value) {
			Value = value;
			MarkAsUpdated();
		}

		public void MarkAsUpdated() {
			Version = Guid.NewGuid().ToString();
		}

		public static implicit operator T(Versioned<T> versioned) => versioned.Value;
	}
}