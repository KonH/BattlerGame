using GameLogics.Server.Repositories.Configs;
using GameLogics.Server.Repositories.States;
using GameLogics.Server.Repositories.Users;
using GameLogics.Server.Services;
using GameLogics.Server.Services.Token;
using GameLogics.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

		public static void AddApiService(this IServiceCollection services) {
			services.AddSingleton<IConfigRepository, InMemoryConfigRepository>();
			services.AddSingleton<ConvertService>();
			services.AddSingleton<IApiService, ServerApiService>();
			services.AddSingleton<ActionResultWrapper>();
		}
		
		public static void AddAuthService(this IServiceCollection services) {
			var settings = new AuthSettings("BattlerServer", "BattlerClient", 60, "wiySRZgKELlQcN82");
			services.AddSingleton(settings);
			services.AddSingleton<ITokenService, JwtTokenService>();
			services.AddSingleton<AuthService>();
			services.AddJwtBearerAuthentication(settings);
			services.AddSingleton<StateInitService>();
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
		
		public static void AddUserServices(this IServiceCollection services) {
			services.AddSingleton<IUsersRepository, InMemoryUsersRepository>();
			services.AddSingleton<RegisterService>();
		}
		
		public static void AddGameStateRepository(this IServiceCollection services) {
			services.AddSingleton<IGameStatesRepository, InMemoryGameStatesRepository>();
		}
		
		public static void AddIntentService(this IServiceCollection services) {
			services.AddSingleton<IntentService>();
		}
	}
}