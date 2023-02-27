using Bloggy.Service.PostService;
using Bloggy.Shared.Config;
using Bloggy.Shared.Model;

namespace Bloggy.Service.DynamicPageService
{
    public interface IDynamicPageService
    {
        Task<bool> CheckSlugIsDuplicated(string Slug, int Id);
        Task<bool> Delete(int Id, CancellationToken cancellationToken);
        Task<DynamicPageInsertDto> GetById(int Id, CancellationToken cancellationToken);
        Task<DynamicPageSelectDto> GetBySlug(string Slug, CancellationToken cancellationToken);
        Task<int> GetTotalItemCount();
        Task<List<DynamicPageListSelectDto>> List(Pageres arg, CancellationToken cancellationToken);
        Task<ResponseModel> Submit(DynamicPageInsertDto dto, CancellationToken cancellationToken);
    }
}
