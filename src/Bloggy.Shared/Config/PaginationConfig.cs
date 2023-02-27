namespace Bloggy.Shared.Config
{
    public interface IPaginationInfo
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }

    public class Pageres : IPaginationInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, IPaginationInfo pagination)
        {
            if (pagination.PageSize > 100)
                pagination.PageSize = 100;
            if (pagination.PageSize == 0)
                pagination.PageSize = 10;
            if (pagination.PageNumber == 0)
                pagination.PageNumber = 1;
            return source
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize);
        }
    }
}
