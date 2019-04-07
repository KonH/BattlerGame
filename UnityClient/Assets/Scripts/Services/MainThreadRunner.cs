using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityClient.Services {
	public sealed class MainThreadRunner : MonoBehaviour {
		TaskScheduler _scheduler;
		
		void Awake() {
			_scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		}

		public void Run(Func<Task> task) {
			Task.Factory.StartNew(async () => {
				try {
					await task();
				} catch ( Exception e ) {
					Debug.LogErrorFormat("MainThreadRunner.Run: {0}", e);
				}
			}, CancellationToken.None, TaskCreationOptions.None, _scheduler);
		}
	}

}