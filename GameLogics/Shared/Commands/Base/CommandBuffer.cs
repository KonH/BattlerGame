using System.Collections.Generic;

namespace GameLogics.Shared.Commands.Base {
	public sealed class CommandBuffer : ICommandBuffer {
		List<ICommand> _buffer = new List<ICommand>();
		
		public void Add(ICommand command) {
			_buffer.Add(command);
		}

		public ICommand this[int index] => (index < _buffer.Count) ? _buffer[index] : null;
	}
}