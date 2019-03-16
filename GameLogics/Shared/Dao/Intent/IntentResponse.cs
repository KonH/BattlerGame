namespace GameLogics.Shared.Dao.Intent {
	public class IntentResponse {
		public string NewVersion { get; set; }

		public IntentResponse(string newVersion) {
			NewVersion = newVersion;
		}

		public override string ToString() {
			return $"NewVersion: '{NewVersion}'";
		}
	}
}