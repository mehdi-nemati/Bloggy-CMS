using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bloggy.Entities
{
    public class DynamicPage : BaseEntity
    {
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public string Slug { get; set; }
    }

    public class DynamicPageConfiguration : IEntityTypeConfiguration<DynamicPage>
    {
        public void Configure(EntityTypeBuilder<DynamicPage> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(100).IsRequired();
            builder.Property(e => e.ShortContent).HasMaxLength(160);
        }
    }
}
