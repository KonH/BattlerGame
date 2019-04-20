using GameLogics.Server.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server {
	public sealed class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddFullCors();
			services.AddLogging();
			services.AddCustomLogger();
			services.AddApiService();
			services.AddAuthService();
			services.AddUserServices();
			services.AddGameStateRepository();
			services.AddIntentService();
			services.AddTimeService();
			services.AddEnvironmentService();
			services.AddMvc(opts =>
				opts.EnableEndpointRouting = false
			).AddNewtonsoftJson(opts =>
				opts.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
			);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			app.UseDeveloperExceptionPage();
			app.ApplicationServices.GetService<EnvironmentService>().IsDebugMode = true;
			app.UseFullCors();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}