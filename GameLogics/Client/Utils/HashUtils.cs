using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GameLogics.Client.Utils {
	public static class HashUtils {
		public static string MakePasswordHash(string login, string password) {
			return MakeHash(login + password);
		}

		public static string MakeHash(string input) {
			var encoding   = Encoding.UTF8;
			var inputData  = encoding.GetBytes(input);
			var provider   = new SHA1CryptoServiceProvider();
			var outputData = provider.ComputeHash(inputData);
			var output     = Convert.ToBase64String(outputData);
			var safeOutput = output.Where(c => char.IsLetterOrDigit(c)).ToArray();
			return new string(safeOutput);
		}
	}
}