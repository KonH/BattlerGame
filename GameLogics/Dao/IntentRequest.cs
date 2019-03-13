using GameLogics.Intents;

namespace GameLogics.DAO {
	public class IntentRequest {
		public string  ExpectedVersion { get; set; }
		public IIntent Intent          { get; set; }

		public IntentRequest(string expectedVersion, IIntent intent) {
			ExpectedVersion = expectedVersion;
			Intent          = intent;
		}
		
		public override string ToString() {
			return $"ExpectedVersion: '{ExpectedVersion}', Intent: {Intent}";
		}
	}
}