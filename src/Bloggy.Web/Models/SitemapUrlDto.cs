
namespace Bloggy.Web.Models
{
    public class SitemapUrlDto
    {
        public string Url { get; set; }
        public DateTime? Modified { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public double? Priority { get; set; }
    }

    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}
