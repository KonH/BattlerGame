using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameLogics.Server.Model;
using GameLogics.Server.Service.Token;
using Microsoft.IdentityModel.Tokens;
using Server.Settings;

namespace Server.Service {
	public sealed class JwtTokenService : ITokenService {
		readonly AuthSettings _settings;

		public JwtTokenService(AuthSettings settings) {
			_settings = settings;
		}
		
		public string CreateToken(UserState user) {
			var identity = GetIdentity(user);
			var jwt = CreateToken(identity);
			var encoded = new JwtSecurityTokenHandler().WriteToken(jwt);
			return encoded;
		}
		
		ClaimsIdentity GetIdentity(UserState user) {
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