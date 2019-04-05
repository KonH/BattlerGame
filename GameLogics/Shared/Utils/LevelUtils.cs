namespace GameLogics.Shared.Utils {
	public static class LevelUtils {
		public static string GetScope(string desc) {
			return desc.Substring(0, desc.LastIndexOf('_'));
		}
		
		public static int GetIndex(string desc) {
			var parts = desc.Split('_');
			return int.Parse(parts[parts.Length - 1]);
		}
	}
}