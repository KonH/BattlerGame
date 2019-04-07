using System.Collections.Generic;
using GameLogics.Shared.Services;
using UnityClient.Models;
using JetBrains.Annotations;

namespace UnityClient.Services {
	public sealed class NoticeService {
		readonly ICustomLogger _logger;
		
		Stack<NoticeModel> _notices = new Stack<NoticeModel>();
		
		public NoticeService(ICustomLogger logger) {
			_logger = logger;
		}

		public void ScheduleNotice(NoticeModel notice) {
			_logger.DebugFormat(this, "ScheduleNotice: '{0}'", notice);
			_notices.Push(notice);
		}

		[CanBeNull]
		public NoticeModel RequestNotice() {
			if ( _notices.Count > 0 ) {
				return _notices.Pop();
			}
			return null;
		}
	}
}
