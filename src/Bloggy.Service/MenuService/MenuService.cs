using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Shared;
using Bloggy.Shared.Config;
using Bloggy.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Bloggy.Service.MenuService
{
	public class MenuService : IScopedDependency, IMenuService
	{
		private readonly IMapper _mapper;
		private readonly IRepository<Menu> _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public MenuService(IMapper mapper, IRepository<Menu> repository, IStringLocalizer<SharedResources> localizer)
        {
            _mapper = mapper;
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<List<MenuWithChildrenSelecDto>> GetMenuWithChildren(CancellationToken cancellationToken)
		{
			return await _repository.TableNoTracking.OrderBy(a => a.Id).ProjectTo<MenuWithChildrenSelecDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
		}

		public async Task<MenuDto> Get(int Id,CancellationToken cancellationToken)
		{
			return await _repository.TableNoTracking.OrderBy(a => a.Id).ProjectTo<MenuDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(m=>m.Id == Id,cancellationToken);
		}

		public async Task<bool> CreateMenu(MenuDto dto, CancellationToken cancellationToken)
		{
            if (dto.Id == 0)
            {
                var model = _mapper.Map<Menu>(dto);
                model.ParentId = dto.ParentId == 0 ? null : dto.ParentId;

                await _repository.AddAsync(model, cancellationToken);
            }
            else
            {
                var findedItem = await _repository.Table.FirstOrDefaultAsync(m => m.Id == dto.Id);
                if (findedItem != null)
                {
                    findedItem.ParentId = dto.ParentId == 0 ? null : dto.ParentId;
                    var entity = _mapper.Map(dto, findedItem);
                    await _repository.UpdateAsync(entity, cancellationToken);
                }
                else { return false; }
            }
            return true;
        }

        public async Task<ResponseModel> Delete(int Id, CancellationToken cancellationToken)
        {
            var findedCat = await _repository.Table.Include(m=>m.Children).FirstOrDefaultAsync(m => m.Id == Id, cancellationToken);
            if (findedCat != null)
            {
                if (findedCat.Children.Count != 0)
                {
                    return new ResponseModel(false, _localizer["parent-with-child-error"]);
                }
                await _repository.DeleteAsync(findedCat, cancellationToken);
                return new ResponseModel(true);
            }
            else { return new ResponseModel(false, _localizer["not-found"]); }
        }
    }
}
