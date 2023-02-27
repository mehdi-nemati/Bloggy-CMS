using Bloggy.Service.AlertService;
using Bloggy.Service.MenuService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuService _MenuService;
        public MenuController(IAlertService alertService, IMenuService MenuService) : base(alertService)
        {
            _MenuService = MenuService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _MenuService.GetMenuWithChildren(cancellationToken));
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var Menus = await _MenuService.GetMenuWithChildren(cancellationToken);
            var PostDto = new MenuDto
            {
                ParentCategory = new SelectList(Menus.Where(m => m.ParentId == null), "Id", "Title")
            };
            return View("add", PostDto);
        }

        public async Task<IActionResult> Update(int id, CancellationToken cancellationToken)
        {
            var FindedItem = await _MenuService.Get(id, cancellationToken);
            var Menus = await _MenuService.GetMenuWithChildren(cancellationToken);

            FindedItem.ParentCategory = new SelectList(Menus.Where(m => m.ParentId == null), "Id", "Title", FindedItem.ParentId);
            return View("add", FindedItem);
        }

        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var res = await _MenuService.Delete(Id, cancellationToken);

            if (res.IsSuccess)
                CreateSuccessAlert();
            else
                CreateAlert("", res.Message, AlertIcons.error);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(MenuDto dto, CancellationToken cancellationToken)
        {
            var res = await _MenuService.CreateMenu(dto, cancellationToken);
            if (res)
            {
                CreateSuccessAlert();
                return RedirectToAction(nameof(Index));
            }
            CreateNotSuccessAlert();
            return View("add", dto);
        }
    }
}
