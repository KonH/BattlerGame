using System;
using System.Collections.Generic;

namespace GameLogics.Shared.Utils {
	public static class CollectionExtensions {
		public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
			return dictionary.TryGetValue(key, out var obj) ? obj : defaultValue;
		}
		
		public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) {
			return dictionary.GetOrDefault(key, default(TValue));
		}

		public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newInstance = null) where TValue : class, new() {
			TValue value;
			if ( dictionary.TryGetValue(key, out value) ) {
				return value;
			}
			value = (newInstance != null) ? newInstance() : new TValue();
			dictionary[key] = value;
			return value;
		}
	}
}