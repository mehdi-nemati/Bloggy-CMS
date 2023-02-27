namespace Bloggy.Shared.Model
{
    public class ListModel<T>
    {
        public int ItemCount { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
    }
}
