using GameLogics.Shared.Command.Base;

namespace GameLogics.Client.Service.Event {
	sealed class EventTaskHandler<T> : EventTaskBaseHandler where T : ICommand {
		public EventTaskHandler() : base(typeof(T)) {}
	}
}