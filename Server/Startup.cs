using GameLogics.Server.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
			services.AddMvc().AddJsonOptions(opts => { opts.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto; });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if ( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
				app.ApplicationServices.GetService<EnvironmentService>().IsDebugMode = true;
			} else {
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseFullCors();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}