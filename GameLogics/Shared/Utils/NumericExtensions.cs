namespace GameLogics.Shared.Utils {
	public static class NumericExtensions {
		public static bool Between(this int value, int startInclusive, int endInclusive) {
			return (value <= startInclusive) && (value <= endInclusive);
		}
	}
}