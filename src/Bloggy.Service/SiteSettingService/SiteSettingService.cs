using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Service.UploadService;
using Bloggy.Shared;
using Bloggy.Shared.Config;
using Bloggy.Shared.Extension;
using Bloggy.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Bloggy.Service.SiteSettingService
{
    public class SiteSettingService : IScopedDependency, ISiteSettingService
    {
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IRepository<SiteSetting> _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public SiteSettingService(IRepository<SiteSetting> repository, IMapper mapper, IStringLocalizer<SharedResources> localizer, IUploadService uploadService)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _uploadService = uploadService;
        }
        public async Task<ResponseModel> Submit(SiteSettingInsertDto dto, CancellationToken cancellationToken)
        {
            if (!dto.SiteFavIconFile.IsImage() || !dto.SiteLogoFile.IsImage())
            {
                return new ResponseModel(false, _localizer["file-type-is-not-valid"]);
            }

            if (dto.SiteFavIconFile != null)
                dto.SiteFavIcon = await _uploadService.UploadFile(dto.SiteFavIconFile, "sitesetting");

            if (dto.SiteLogoFile != null)
                dto.SiteLogo = await _uploadService.UploadFile(dto.SiteLogoFile, "sitesetting");

            if (dto.Id == 0)
            {
                await _repository.AddAsync(_mapper.Map<SiteSetting>(dto), cancellationToken);
            }
            else
            {
                var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == dto.Id, cancellationToken: cancellationToken);
                if (findedItem != null)
                {
                    var entity = _mapper.Map(dto, findedItem);
                    await _repository.UpdateAsync(entity, cancellationToken);
                }
                else { return new ResponseModel(false, _localizer["not-found"]); }
            }
            return new ResponseModel(true);
        }

        public async Task<SiteSettingInsertDto> GetFirstItemForEdit(CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<SiteSettingInsertDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task RemoveLogoPicture(int Id, CancellationToken cancellationToken)
        {
            var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken: cancellationToken);
            if (findedItem != null && !string.IsNullOrEmpty(findedItem.SiteLogo))
            {
                _uploadService.DeleteFileFromStorage(findedItem.SiteLogo);

                findedItem.SiteLogo = null;
                await _repository.UpdateAsync(findedItem, cancellationToken, true);
            }
        }

        public async Task RemoveFavIconPicture(int Id, CancellationToken cancellationToken)
        {
            var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken: cancellationToken);
            if (findedItem != null && !string.IsNullOrEmpty(findedItem.SiteFavIcon))
            {
                _uploadService.DeleteFileFromStorage(findedItem.SiteFavIcon);

                findedItem.SiteFavIcon = null;
                await _repository.UpdateAsync(findedItem, cancellationToken, true);
            }
        }
    }
}
