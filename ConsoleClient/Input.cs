using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleClient {
	public static class Input {
		public static T Read<T>(string request, Func<string, (bool, T)> convert) {
			while ( true ) {
				Console.Write($"{request}: ");
				var input = Console.ReadLine();
				var (success, value) = convert(input);
				if ( success ) {
					return value;
				}
			}
		}

		public static int ReadScopedInt(string request, int min, int max) {
			return Read(request, s => int.TryParse(s, out var value) && (value >= min) && (value <= max) ? (true, value) : (false, -1));
		}

		public static string ReadString(string request) {
			return Read(request, s => (true, s));
		}

		public static object ReadArbitraryValue(string request, Type type) {
			return Read(request, s => TryParse(type, s));
		}

		static (bool, object) TryParse(Type type, string s) {
			if ( type == typeof(string) ) {
				return (true, s);
			}
			if ( type == typeof(ulong) ) {
				return ulong.TryParse(s, out var value) ? (true, value) : (false, 0);
			}
			if ( type == typeof(int) ) {
				return int.TryParse(s, out var value) ? (true, value) : (false, 0);
			}
			if ( type.IsEnum ) {
				return Enum.TryParse(type, s, out var value) ? (true, value) : (false, null);
			}
			if ( type == typeof(DateTime) ) {
				return DateTime.TryParse(s, out var value) ? (true, value) : (false, default);
			}
			if ( type == typeof(TimeSpan) ) {
				return TimeSpan.TryParse(s, out var value) ? (true, value) : (false, default);
			}
			if ( type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)) ) {
				var elementType = type.GetGenericArguments()[0];
				var addMethod = type.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
				var instance = Activator.CreateInstance(type);
				var parts = s.Split(',');
				foreach ( var part in parts ) {
					var (success, value) = TryParse(elementType, part);
					if ( !success ) {
						return (false, null);
					}
					addMethod.Invoke(instance, new [] { value });
				}
				return (true, instance);
			}
			throw new InvalidOperationException($"Unknown type: {type}");
		}
	}
}