using GameLogics.Intents;

namespace GameLogics.DAO {
	public class IntentRequest {
		public IIntent Intent { get; set; }
		
		public override string ToString() {
			return $"Intent: {Intent}";
		}
	}
}