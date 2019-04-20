using System;

namespace UnityClient.Model {
	public sealed class ClickAction<T> {
		public string    Name         { get; }
		public Action<T> Callback     { get; }
		public bool      Interactable { get; }

		public ClickAction(string name, Action<T> callback, bool interactable = true) {
			Name         = name;
			Callback     = callback;
			Interactable = interactable;
		}
	}
}