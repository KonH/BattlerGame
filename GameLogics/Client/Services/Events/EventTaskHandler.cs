using GameLogics.Shared.Commands.Base;

namespace GameLogics.Client.Services.Events {
	sealed class EventTaskHandler<T> : EventTaskBaseHandler where T : ICommand {
		public EventTaskHandler() : base(typeof(T)) {}
	}
}