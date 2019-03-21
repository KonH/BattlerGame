using GameLogics.Shared.Commands.Base;

namespace GameLogics.Shared.Dao.Intent {
	public class IntentRequest {
		public string            Login           { get; set; }
		public string            ExpectedVersion { get; set; }
		public ICompositeCommand Command         { get; set; }

		public IntentRequest(string login, string expectedVersion, ICompositeCommand command) {
			Login           = login;
			ExpectedVersion = expectedVersion;
			Command         = command;
		}
		
		public override string ToString() {
			return $"Login: '{Login}', ExpectedVersion: '{ExpectedVersion}', Command: {Command}";
		}
	}
}