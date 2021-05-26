using Identity.Core;
using Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
    .UseLogging()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
}

CreateHostBuilder(args).Build().RunAndLog();
