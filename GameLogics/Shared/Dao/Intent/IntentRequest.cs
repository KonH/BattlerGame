using GameLogics.Shared.Intents;

namespace GameLogics.Shared.Dao.Intent {
	public class IntentRequest {
		public string  Login           { get; set; }
		public string  ExpectedVersion { get; set; }
		public IIntent Intent          { get; set; }

		public IntentRequest(string login, string expectedVersion, IIntent intent) {
			Login           = login;
			ExpectedVersion = expectedVersion;
			Intent          = intent;
		}
		
		public override string ToString() {
			return $"Login: '{Login}', ExpectedVersion: '{ExpectedVersion}', Intent: {Intent}";
		}
	}
}