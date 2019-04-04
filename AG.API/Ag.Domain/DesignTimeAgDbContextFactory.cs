using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ag.Domain
{
    public class DesignTimeAgDbContextFactory : IDesignTimeDbContextFactory<AgDbContext>
    {
        public AgDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory() + @"\..\Ag.Web\") // TODO: base path should not be hard coded
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AgDbContext>();
            var connectionString = configuration.GetConnectionString("Ag");
            optionsBuilder.UseMySql(connectionString);

            return new AgDbContext(optionsBuilder.Options);
        }
    }
}
