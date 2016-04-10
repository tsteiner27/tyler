using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TylerSteiner.Core.Options;
using TylerSteiner.Services;
using TylerSteiner.Services.Data;

namespace TylerSteiner
{
    public class Startup
    {
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("secrets.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseConfig>(Configuration.GetSection("Database"));

            services.AddMvc();
            services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddTransient<IMovieService, MovieService>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/");
            }

            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}