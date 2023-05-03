using Bloggy.Entities;
using Microsoft.Extensions.Logging;

namespace Bloggy.Data
{
    public class BloggyDbContextSeed
    {
        public static async Task SeedAsync(BloggyDbContext bloggyDbContextContext, ILogger<BloggyDbContextSeed> logger)
        {
            if (!bloggyDbContextContext.Set<SiteSetting>().Any())
            {
                bloggyDbContextContext.Set<SiteSetting>().Add(new SiteSetting
                {
                    CreateDate = DateTime.Now,
                    SiteDesc = "Bloggy CMS",
                    SiteTitle = "Bloggy CMS",
                });
                await bloggyDbContextContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(BloggyDbContext).Name);
            }
        }
    }
}
