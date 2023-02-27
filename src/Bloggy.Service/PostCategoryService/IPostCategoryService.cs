namespace Bloggy.Service.PostCategoryService
{
    public interface IPostCategoryService
    {
        Task<bool> Submit(PostCategoryInsertDto dto, CancellationToken cancellationToken);
        Task<List<PostCategorySelectDto>> List(CancellationToken cancellationToken);
        Task<PostCategoryInsertDto> GetById(int Id, CancellationToken cancellationToken);
        Task<bool> Delete(int Id, CancellationToken cancellationToken);
    }
}
