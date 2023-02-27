using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bloggy.Data
{
    public static class AddDbContextExtention
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string DbProvider = configuration["Bloggy:DbProvider"];
            string ConnectionString = configuration["Bloggy:ConnectionString"];

            if (DbProvider == "SqlServer")
                services.AddDbContext<BloggyDbContext>(o => o.UseSqlServer(ConnectionString));

            if (DbProvider == "SQLite")
                services.AddDbContext<BloggyDbContext>(o => o.UseSqlite(ConnectionString));

            return services;
        }
    }
}
