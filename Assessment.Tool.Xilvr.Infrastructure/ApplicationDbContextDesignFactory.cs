using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Assessment.Tool.Xilvr.Infrastructure;

public class ApplicationDbContextDesignFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Assessment.Tool.Xilvr.Api"))
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        var connectionString = configuration.GetConnectionString("DbConnection");
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new ApplicationContext(optionsBuilder.Options);
    }
}
