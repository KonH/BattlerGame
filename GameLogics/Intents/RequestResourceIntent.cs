using System;
using GameLogics.Core;

namespace GameLogics.Intents {
	public class RequestResourceIntent : IIntent {
		public readonly Resource Kind;
		public readonly int      Count;
		
		public RequestResourceIntent(Resource kind, int count) {
			Kind  = (kind != Resource.Unknown) ? kind : throw new InvalidOperationException(nameof(kind));
			Count = count;
		}
		
		public override string ToString() {
			return string.Format("{0} ({1}, {2})", nameof(RequestResourceIntent), Kind, Count);
		}
	}
}