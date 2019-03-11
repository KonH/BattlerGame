using System;
using System.Threading.Tasks;

namespace UnityClient.Utils {
	public static class TaskExtensions {
		public static void ContinueOnSameThread<T>(this Task<T> task, Action<Task<T>> continuation) {
			task.ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}