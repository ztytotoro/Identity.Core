using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database
{
    public static class DatabaseExtension
    {
        public static void AddNpgsqlDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IdentityCoreContext>(options =>
                    options.UseNpgsql(connectionString));
        }

        public static void UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetService<IdentityCoreContext>();

            context.Database.EnsureCreated();
        }
    }
}
