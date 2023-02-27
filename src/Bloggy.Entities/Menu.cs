using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Entities
{
	public class Menu : BaseEntity
	{
		public string Title { get; set; }
		public string Url { get; set; }
		public byte Order { get; set; }
		public int? ParentId { get; set; }
		public Menu ParentMenu { get; set; }
		public List<Menu> Children { get; set; }
	}

	public class MenuConfiguration : IEntityTypeConfiguration<Menu>
	{
		public void Configure(EntityTypeBuilder<Menu> builder)
		{
			builder.Property(a => a.Title).HasMaxLength(100);
			builder.Property(a => a.Url).HasMaxLength(200);
			builder.HasOne(a => a.ParentMenu).WithMany(m => m.Children).HasForeignKey(m => m.ParentId).IsRequired(false);
		}
	}
}
