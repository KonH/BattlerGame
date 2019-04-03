using System;

namespace UnityClient.Models {
	public class ClickAction<T> {
		public string    Name     { get; }
		public Action<T> Callback { get; }

		public ClickAction(string name, Action<T> callback) {
			Name     = name;
			Callback = callback;
		}
	}
}