using Bloggy.Shared.Model;

namespace Bloggy.Service.SiteSettingService
{
    public interface ISiteSettingService
    {
        Task<SiteSettingInsertDto> GetFirstItemForEdit(CancellationToken cancellationToken);
        Task<ResponseModel> Submit(SiteSettingInsertDto dto, CancellationToken cancellationToken);
        Task RemoveLogoPicture(int Id, CancellationToken cancellationToken);
        Task RemoveFavIconPicture(int Id, CancellationToken cancellationToken);
    }
}
