using System;
using System.Collections;
using System.Collections.Generic;
using GameLogics.Shared.Models;
using GameLogics.Shared.Models.Configs;

namespace GameLogics.Shared.Commands.Base {
	public abstract class BaseCommand : ICompositeCommand, IPendingCommand {
		
		// To prevent base command from serialization as enumerable
		public class Enumerable : IEnumerable<IPendingCommand> {
			readonly BaseCommand _owner;
			
			public Enumerable(BaseCommand owner) {
				_owner = owner;
			}
			
			public IEnumerator<IPendingCommand> GetEnumerator() {
				return _owner.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}
		}
		
		class Enumerator : IEnumerator<IPendingCommand> {
			public IPendingCommand Current => _buffer.TryGetAt(_position);
			
			object IEnumerator.Current => Current;

			readonly CommandBuffer _buffer;

			int _position = -1;

			public Enumerator(BaseCommand firstCommand) {
				_buffer = new CommandBuffer(firstCommand);
			}
			
			public bool MoveNext() {
				_position++;
				var command = _buffer.TryGetAt(_position);
				if ( command == null ) {
					// No commands in buffer
					return false;
				}
				// Preparing for next call
				command.SetupBuffer(_buffer);
				return true;
			}

			public void Reset() {
				throw new NotSupportedException();
			}

			public void Dispose() {}
		}
		
		ICommandBuffer _buffer = null;

		public bool IsFirstCommandValid(GameState state, Config config) {
			return IsValid(state, config);
		}

		public IEnumerable<IPendingCommand> AsEnumerable() {
			return new Enumerable(this);
		}
		
		protected abstract bool IsValid(GameState state, Config config);
		protected abstract void Execute(GameState state, Config config, ICommandBuffer buffer);

		IEnumerator<IPendingCommand> GetEnumerator() {
			if ( _buffer != null ) {
				throw new InvalidOperationException("Enumeration already started!");
			}
			return new Enumerator(this);
		}

		bool IPendingCommand.IsCommandValid(GameState state, Config config) {
			if ( _buffer == null ) {
				throw new InvalidOperationException("Can be called only through enumerator!");
			}
			return IsValid(state, config);
		}

		void IPendingCommand.ExecuteCommand(GameState state, Config config) {
			if ( _buffer == null ) {
				throw new InvalidOperationException("Can be called only through enumerator!");
			}
			Execute(state, config, _buffer);
		}
		
		void SetupBuffer(ICommandBuffer buffer) {
			_buffer = buffer;
		}
	}
}