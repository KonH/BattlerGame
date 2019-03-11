using GameLogics.Core;
using GameLogics.Managers;
using GameLogics.Managers.IntentMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Repositories;
using Server.Services;
using Server.Settings;

namespace Server {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddFullCors();
			services.AddLogging();
			services.AddCustomLogger();
			services.AddAuthService();
			services.AddUserRepository();
			services.AddGameStateManager();
			services.AddIntentService();
			services.AddMvc().AddJsonOptions(opts => { opts.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto; });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if ( env.IsDevelopment() ) {
				app.UseDeveloperExceptionPage();
			} else {
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseFullCors();
			app.UseAuthentication();
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}