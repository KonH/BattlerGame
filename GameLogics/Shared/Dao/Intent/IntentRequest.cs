using GameLogics.Shared.Commands;

namespace GameLogics.Shared.Dao.Intent {
	public class IntentRequest {
		public string     Login           { get; set; }
		public string     ExpectedVersion { get; set; }
		public ICommand[] Commands        { get; set; }

		public IntentRequest(string login, string expectedVersion, ICommand[] commands) {
			Login           = login;
			ExpectedVersion = expectedVersion;
			Commands        = commands;
		}
		
		public override string ToString() {
			return $"Login: '{Login}', ExpectedVersion: '{ExpectedVersion}', Commands: {string.Join<object>(";", Commands)}";
		}
	}
}