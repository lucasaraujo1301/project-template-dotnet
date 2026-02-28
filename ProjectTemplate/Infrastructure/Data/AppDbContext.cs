using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ProjectTemplate.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        // public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        // {
        //     public AppDbContext CreateDbContext(string[] args)
        //     {
        //         string jsonFile = "appsettings.json";

        //         string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        //         if (!string.IsNullOrEmpty(environment) && environment != "Production")
        //         {
        //             jsonFile = $"appsettings.{environment}.json";
        //         }

        //         IConfigurationRoot configuration = new ConfigurationBuilder()
        //             .SetBasePath(Directory.GetCurrentDirectory())
        //             .AddJsonFile(jsonFile) // Adjust the path if needed
        //             .AddEnvironmentVariables()
        //             .AddUserSecrets<Program>()
        //             .Build();

        //         var connectionString = configuration.GetConnectionString("DefaultConnection");

        //         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //         optionsBuilder.UseNpgsql(connectionString);

        //         return new AppDbContext(optionsBuilder.Options);
        //     }
        // }
    }

}