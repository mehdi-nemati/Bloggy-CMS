using Bloggy.Service.AlertService;
using Bloggy.Service.PostCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class PostCategoryController : BaseController
    {
        private readonly IPostCategoryService _PostCategoryService;
        public PostCategoryController(IAlertService alertService, IPostCategoryService postCategoryService) : base(alertService)
        {
            _PostCategoryService = postCategoryService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View(await _PostCategoryService.List(cancellationToken));
        }

        public IActionResult Create()
        {
            return View("add", new PostCategoryInsertDto());
        }

        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            return View("add", await _PostCategoryService.GetById(Id, cancellationToken));
        }

        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var res = await _PostCategoryService.Delete(Id, cancellationToken);
            if (res)
                CreateSuccessAlert();
            else CreateNotSuccessAlert();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PostCategoryInsertDto dto, CancellationToken cancellationToken)
        {
            if (await _PostCategoryService.Submit(dto, cancellationToken))
            {
                CreateSuccessAlert();
                return RedirectToAction(nameof(Index));
            }
            CreateNotSuccessAlert();
            return View("add", dto);
        }
    }
}
