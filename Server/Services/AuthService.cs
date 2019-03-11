using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameLogics.Models;
using Microsoft.IdentityModel.Tokens;
using Server.Settings;

namespace Server.Services {
	public class AuthService {
		readonly AuthSettings _settings;

		public AuthService(AuthSettings settings) {
			_settings = settings;
		}
		
		public JwtSecurityToken CreateToken(User user) {
			var identity = GetIdentity(user);
			return CreateToken(identity);
		}
		
		ClaimsIdentity GetIdentity(User user) {
			var claims = new List<Claim> {
				new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
			};
			return new ClaimsIdentity(
				claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType
			);
		}
		
		JwtSecurityToken CreateToken(ClaimsIdentity identity) {
			var now = DateTime.UtcNow;
			return new JwtSecurityToken(
				issuer            : _settings.Issuer,
				audience          : _settings.Audience,
				notBefore         : now,
				claims            : identity.Claims,
				expires           : now.Add(TimeSpan.FromMinutes(_settings.Lifetime)),
				signingCredentials: new SigningCredentials(_settings.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
		}
	}
}