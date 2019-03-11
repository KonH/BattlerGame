using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Settings {
	public class AuthSettings {
		public string Issuer   { get; }
		public string Audience { get; }
		public int    Lifetime { get; }
		public string Key      { get; }
		
		public AuthSettings(string issuer, string audience, int lifetime, string key) {
			Issuer   = issuer;
			Audience = audience;
			Lifetime = lifetime;
			Key      = key;
		}
		
		public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
	}
}