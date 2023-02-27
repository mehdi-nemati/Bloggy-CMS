using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bloggy.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public string CoverImage { get; set; }

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public string Slug { get; set; }

        public int PostCategoryId { get; set; }
        public PostCategory PostCategory { get; set; }

        public int CreatorUserId { get; set; }
        public ApplicationUser CreatorUser { get; set; }
    }

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(100).IsRequired();
            builder.Property(e => e.ShortContent).HasMaxLength(160);
            builder.HasOne(m => m.PostCategory).WithMany(m => m.Posts).HasForeignKey(m => m.PostCategoryId);
            builder.HasOne(m => m.CreatorUser).WithMany(m => m.Posts).HasForeignKey(m => m.CreatorUserId);
        }
    }
}
