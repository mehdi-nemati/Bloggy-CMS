namespace Bloggy.Entities
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
