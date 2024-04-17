namespace API.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using API.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "appsettings.Development.json");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appSettingsPath)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = configBuilder.GetConnectionString("DBcon");

            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
