using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Entities
{
    public class PostCategory : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(60).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(160);
        }
    }
}
