using Microsoft.AspNetCore.Identity;

namespace Bloggy.Entities
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string DisplayName{ get; set; }
        public string DiplayProfilePicture{ get; set; }

        public List<Post> Posts { get; set; }
    }
}
