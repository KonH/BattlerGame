using System.Collections.Generic;

namespace GameLogics.Utils {
	public static class CollectionExtensions {
		public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
			return dictionary.TryGetValue(key, out var obj) ? obj : defaultValue;
		}
		
		public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) {
			return dictionary.GetOrDefault(key, default(TValue));
		}
	}
}