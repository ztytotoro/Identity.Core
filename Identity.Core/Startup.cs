using ApiDocument;
using Database;
using ErrorHandler;
using Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace Identity.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // [ADD] Use Database
            services.AddNpgsqlDatabase(Configuration.GetConnectionString("IdentityCoreContextConnection"));

            // [ADD] Configure Identity with JWT token
            services.AddJwtIdentity(new JwtOptions
            {
                Issuer = Configuration["Token:Issuer"],
                Audience = Configuration["Token:Audience"],
                Key = Configuration["Token:Key"]
            });

            // [ADD] Localization
            services.AddErrorLocalization();

            // [ADD] Add Api Document
            services.AddApiDocument();

            // [ADD] Add Error Handler
            services.AddControllers(options => options.UseExceptionFilter());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // [ADD] Show PII
                IdentityModelEventSource.ShowPII = true;
            }

            // [ADD] Serilog for request
            app.UseRequestLogging();

            // [ADD] Localization
            app.UseErrorLocalization();

            // [ADD] Use open api documents
            app.UseApiDocument();

            app.UseRouting();

            // [ADD] Add Identity features
            app.UseIdentity();

            // [ADD] Automatic migration when startup
            app.UseMigration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
