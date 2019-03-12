using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityClient.Managers {
	public class MainThreadRunner : MonoBehaviour {
		TaskScheduler _scheduler;
		
		void Awake() {
			_scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		}

		public void Run(Func<Task> task) {
			Task.Factory.StartNew(task, CancellationToken.None, TaskCreationOptions.None, _scheduler);
		}
	}

}