using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLogics.Shared.Commands.Base;

namespace GameLogics.Client.Services.Events {
	abstract class EventTaskBaseHandler {
		List<object> _instances = new List<object>();
		MethodInfo   _method    = null;
		object[]     _args      = { null };

		protected EventTaskBaseHandler(Type type) {
			var openType   = typeof(Func<,>);
			var closedType = openType.MakeGenericType(type, typeof(Task));
			_method = closedType.GetMethod("Invoke");
		}
		
		public Task Invoke(object handler, ICommand item) {
			_args[0] = item;
			var task = (Task)_method.Invoke(handler, _args);
			_args[0] = null;
			return task;
		}

		public void Add(object handler) {
			_instances.Add(handler);
		}

		public void Remove(object handler) {
			_instances.Remove(handler);
		}

		public List<object> GetHandlers() {
			return _instances;
		}
	}
}