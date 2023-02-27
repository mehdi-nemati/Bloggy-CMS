using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Shared;
using Bloggy.Shared.Config;
using Bloggy.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Bloggy.Service.DynamicPageService
{
    public class DynamicPageService : IScopedDependency, IDynamicPageService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<DynamicPage> _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public DynamicPageService(IRepository<DynamicPage> repository, IMapper mapper, IStringLocalizer<SharedResources> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ResponseModel> Submit(DynamicPageInsertDto dto, CancellationToken cancellationToken)
        {
            if (await CheckSlugIsDuplicated(dto.Slug, dto.Id))
                return new ResponseModel(false, _localizer["slug-duplicated"]);

            if (dto.Id == 0)
            {
                await _repository.AddAsync(_mapper.Map<DynamicPage>(dto), cancellationToken);
            }
            else
            {
                var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == dto.Id, cancellationToken: cancellationToken);
                if (findedItem != null)
                {
                    var entity = _mapper.Map(dto, findedItem);
                    await _repository.UpdateAsync(entity, cancellationToken);
                }
                else return new ResponseModel(false, _localizer["not-found"]);
            }
            return new ResponseModel(true);
        }

        public async Task<bool> CheckSlugIsDuplicated(string Slug, int Id)
        {
            var query = _repository.TableNoTracking.Where(m => m.Slug == Slug);
            if (Id != 0)
                query = query.Where(m => m.Id != Id);
            return await query.AnyAsync();
        }

        public async Task<int> GetTotalItemCount()
        {
            return await _repository.TableNoTracking.CountAsync();
        }

        public async Task<List<DynamicPageListSelectDto>> List(Pageres arg, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.OrderByDescending(m => m.Id).Paginate(arg).ProjectTo<DynamicPageListSelectDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }

        public async Task<DynamicPageInsertDto> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<DynamicPageInsertDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => (m.Id == Id), cancellationToken);
        }

        public async Task<DynamicPageSelectDto> GetBySlug(string Slug, CancellationToken cancellationToken)
        {
            return await _repository.TableNoTracking.ProjectTo<DynamicPageSelectDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m => m.Slug == Slug, cancellationToken);
        }

        public async Task<bool> Delete(int Id, CancellationToken cancellationToken)
        {
            var FindedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
            if (FindedItem != null)
            {
                await _repository.DeleteAsync(FindedItem, cancellationToken);
                return true;
            }
            else { return false; }
        }
    }
}