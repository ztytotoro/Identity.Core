using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Module
{
    public interface IModule
    {
        IConfiguration Configuration { get => null; set { } }

        void ConfigureServices(IServiceCollection services) { }
        void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
    }
}
