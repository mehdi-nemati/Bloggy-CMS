using Bloggy.Entities;
using Bloggy.Shared.Config;
using Bloggy.Shared.Extension;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Data
{
    public class BloggyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public BloggyDbContext(DbContextOptions options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
        }

        public override int SaveChanges()
        {
            if (BloggyConst.DoNotChange)
                return 0;
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (BloggyConst.DoNotChange)
                return Task.FromResult(0);
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
