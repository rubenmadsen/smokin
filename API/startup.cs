using API.Controllers;
using API.Data;
using API.Logging;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "appsettings.Development.json");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appSettingsPath)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = configBuilder.GetConnectionString("DBcon");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddLogging(builder =>
            {
                builder.AddProvider(new DatabaseLoggerProvider((category, level) => level >= LogLevel.Information, connectionString));
            });

            services.AddTransient<TodoAppController>();
        }
    }
}
