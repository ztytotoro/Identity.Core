using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Logging
{
    public static class LogExtension
    {
        public static int RunAndLog(this IHost host)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File("Logs/log.txt")
            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog();
        }

        public static void UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }
    }


}
