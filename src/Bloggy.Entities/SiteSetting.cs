using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Entities
{
    public class SiteSetting : BaseEntity
    {
        public string SiteTitle { get; set; }
        public string SiteDesc { get; set; }
        public string SiteLogo { get; set; }
        public string SiteFavIcon { get; set; }
        public string SideMenuCategoryTitle { get; set; }
        public int? SideMenuCategoryId { get; set; }
    }

    public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
    {
        public void Configure(EntityTypeBuilder<SiteSetting> builder)
        {
            builder.Property(e => e.SiteTitle).HasMaxLength(100);
            builder.Property(e => e.SiteDesc).HasMaxLength(160);
        }
    }
}
