using Bloggy.Service.AlertService;
using Bloggy.Service.DynamicPageService;
using Bloggy.Service.UploadService;
using Bloggy.Shared.Config;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class DynamicPageController : BaseController
    {
        private readonly IDynamicPageService _DynamicPageService;
        private readonly IUploadService _UploadService;
        public DynamicPageController(IAlertService alertService, IDynamicPageService DynamicPageService, IUploadService uploadService) : base(alertService)
        {
            _DynamicPageService = DynamicPageService;
            _UploadService = uploadService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken, int Page = 1)
        {
            return View(await _DynamicPageService.List(new Pageres { PageNumber = Page, PageSize = 100 }, cancellationToken));
        }

        public IActionResult Create()
        {
            return View("add", new DynamicPageInsertDto());
        }

        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var FindedItem = await _DynamicPageService.GetById(Id, cancellationToken);
            return View("add", FindedItem);
        }

        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _DynamicPageService.Delete(Id, cancellationToken);
            CreateSuccessAlert();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(DynamicPageInsertDto dto, CancellationToken cancellationToken)
        {
            var res = await _DynamicPageService.Submit(dto, cancellationToken);
            if (res.IsSuccess)
            {
                CreateSuccessAlert();
                return RedirectToAction(nameof(Index));
            }
            CreateAlert("", res.Message, AlertIcons.error);
            return View("add", dto);
        }
    }
}
