using Bloggy.Shared.Config;
using Bloggy.Shared.Model;

namespace Bloggy.Service.PostService
{
    public interface IPostService
    {
        Task<List<PostListSelectDto>> List(Pageres arg, CancellationToken cancellationToken, int CategoryId = 0, bool? IsPublished = null, string SearchString = "");
        Task<PostInsertDto> GetByIdForEdit(int Id, CancellationToken cancellationToken);
        Task<PostSelectDto> GetBySlug(string Slug, CancellationToken cancellationToken);
        Task<ResponseModel> Submit(PostInsertDto dto, CancellationToken cancellationToken);
        Task<bool> Delete(int Id, CancellationToken cancellationToken);
        Task RemovePicture(int Id, CancellationToken cancellationToken);
        Task<int> GetTotalPostCount(int CategoryId = 0, bool? IsPublished = null, string SearchString = "");
    }
}
