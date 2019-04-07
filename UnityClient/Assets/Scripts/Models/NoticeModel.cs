using System;

namespace UnityClient.Models {
	public sealed class NoticeModel {
		public string       Message  { get; }
		public Action<bool> Callback { get; }

		public NoticeModel(string message, Action<bool> callback) {
			Message  = message;
			Callback = callback;
		}

		public override string ToString() {
			return Message;
		}
	}
}