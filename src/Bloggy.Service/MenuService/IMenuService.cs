using Bloggy.Shared.Model;

namespace Bloggy.Service.MenuService
{
	public interface IMenuService
	{
		Task<List<MenuWithChildrenSelecDto>> GetMenuWithChildren(CancellationToken cancellationToken);
		Task<bool> CreateMenu(MenuDto placeDto, CancellationToken cancellationToken);
		Task<MenuDto> Get(int Id, CancellationToken cancellationToken);
		Task<ResponseModel> Delete(int Id, CancellationToken cancellationToken);
    }
}
