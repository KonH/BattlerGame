using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Server.Repositories;
using Server.Services;
using Server.Settings;

namespace Server {
	public static class StartupExtensions {
		public static void AddFullCors(this IServiceCollection services) {
			services.AddCors(o => o.AddPolicy("FullCorsPolicy", builder => {
				builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));
		}

		public static void UseFullCors(this IApplicationBuilder app) {
			app.UseCors("FullCorsPolicy");
		}
		
		public static void AddCustomLogger(this IServiceCollection services) {
			services.AddSingleton<ICustomLogger, ServerLogger>();
		}
		
		public static void AddAuthService(this IServiceCollection services) {
			var settings = new AuthSettings("BattlerServer", "BattlerClient", 60, "wiySRZgKELlQcN82");
			services.AddSingleton(settings);
			services.AddSingleton<AuthService>();
			services.AddJwtBearerAuthentication(settings);
		}
		
		static void AddJwtBearerAuthentication(this IServiceCollection services, AuthSettings settings) {
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options => {
						options.RequireHttpsMetadata = false;
						options.TokenValidationParameters = new TokenValidationParameters {
							ValidateIssuer           = true,
							ValidIssuer              = settings.Issuer,
							ValidateAudience         = true,
							ValidAudience            = settings.Audience,
							ValidateLifetime         = true,
							IssuerSigningKey         = settings.SymmetricSecurityKey,
							ValidateIssuerSigningKey = true
						};
					}
				);
		}
		
		public static void AddUserRepository(this IServiceCollection services) {
			services.AddSingleton<IUserRepository, InMemoryUserRepository>();
		}
		
		public static void AddGameStateManager(this IServiceCollection services) {
			services.AddSingleton<IGameStateManager>(new InMemoryGameStateManager(new GameState()));
		}
		
		public static void AddIntentService(this IServiceCollection services) {
			services.AddSingleton<DirectIntentToCommandMapper>();
			services.AddSingleton<IntentService>();
		}
	}
}