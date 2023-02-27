using Bloggy.Service.PostCategoryService;
using Bloggy.Service.PostService;

namespace Bloggy.Web.Models
{
    public class IndexItemsDto
    {
        public List<PostListSelectDto> Posts { get; set; }
        public List<PostListSelectDto> SideMenuItems { get; set; }
        public List<PostCategorySelectDto> Cats { get; set; }
        public string SideMenuTitle { get; set; }
    }
}
