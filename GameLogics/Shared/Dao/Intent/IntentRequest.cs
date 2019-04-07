using GameLogics.Shared.Commands.Base;

namespace GameLogics.Shared.Dao.Intent {
	public sealed class IntentRequest {
		public string   Login           { get; set; }
		public string   ExpectedVersion { get; set; }
		public ICommand Command         { get; set; }

		public IntentRequest(string login, string expectedVersion, ICommand command) {
			Login           = login;
			ExpectedVersion = expectedVersion;
			Command         = command;
		}
		
		public override string ToString() {
			return $"Login: '{Login}', ExpectedVersion: '{ExpectedVersion}', Command: {Command}";
		}
	}
}