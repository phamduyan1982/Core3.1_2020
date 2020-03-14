using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eShopSolution.Data.EF
{
    public class EShopDbContextFactory : IDesignTimeDbContextFactory<EShopDbContext>
    {

        // https://docs.microsoft.com/vi-vn/ef/core/miscellaneous/cli/dbcontext-creation
        public EShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot Configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = Configurations.GetConnectionString("eShopSolutionDb");

                var optionsBuilder = new DbContextOptionsBuilder<EShopDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new EShopDbContext(optionsBuilder.Options);
           
        }
    }
}
