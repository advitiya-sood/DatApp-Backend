using DatApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var env = services.GetRequiredService<IHostEnvironment>();
                    context.Database.Migrate();
                    if (env.IsDevelopment())
                    {
                        logger.LogInformation("Seeding users in development environment.");
                        Seed.SeedUsers(context);
                        logger.LogInformation("Seeding completed successfully.");
                    }
                    else
                    {
                        logger.LogInformation($"Skipping seeding. Current environment: {env.EnvironmentName}");
                    }
                }
                catch (Exception exp)
                {
                    logger.LogError(exp, "An error occurred during database seeding/migration.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
