using Bloggy.Service.AlertService;
using Bloggy.Service.PostCategoryService;
using Bloggy.Service.SiteSettingService;
using Bloggy.Service.UploadService;
using Bloggy.Shared.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class SiteSettingController : BaseController
    {
        private readonly ISiteSettingService _SiteSettingService;
        private readonly IPostCategoryService _PostCategoryService;
        private readonly IUploadService _UploadService;
        public SiteSettingController(IAlertService alertService, ISiteSettingService SiteSettingService, IPostCategoryService PostCategoryService, IUploadService uploadService) : base(alertService)
        {
            _SiteSettingService = SiteSettingService;
            _PostCategoryService = PostCategoryService;
            _UploadService = uploadService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var res = await _SiteSettingService.GetFirstItemForEdit(cancellationToken);
            res ??= new SiteSettingInsertDto();
            res.Categories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title", res.SideMenuCategoryId);
            return View("edit", res);
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var SiteSettingDto = new SiteSettingInsertDto
            {
                Categories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title")
            };
            return View("edit", SiteSettingDto);
        }

        public async Task<IActionResult> Update(CancellationToken cancellationToken)
        {
            var FindedItem = await _SiteSettingService.GetFirstItemForEdit(cancellationToken);
            FindedItem.Categories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title", FindedItem.SideMenuCategoryId);
            return View("edit", FindedItem);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SiteSettingInsertDto dto, CancellationToken cancellationToken)
        {
            var res = await _SiteSettingService.Submit(dto, cancellationToken);
            if (res.IsSuccess)
            {
                CreateSuccessAlert();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                dto.Categories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title", dto.SideMenuCategoryId);
                CreateAlert("", res.Message, AlertIcons.error);
                return View("edit", dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null;

            if (!upload.IsImage())
            {
                var failed = new { uploaded = 0, upload.FileName, string.Empty, error = new { message = "File type not accepted" } };
                return Json(failed);
            }

            var url = await _UploadService.UploadFile(upload, "SiteSetting");
            var success = new { uploaded = 1, url, error = new { message = "image is uploaded" } };

            return Json(success);
        }

        [HttpPost]
        public async Task<bool> DeleteLogoPicture(int? key, CancellationToken cancellationToken)
        {
            if (key == null) return false;
            await _SiteSettingService.RemoveLogoPicture(key.Value, cancellationToken);
            return true;
        }

        [HttpPost]
        public async Task<bool> DeleteFavIcon(int? key, CancellationToken cancellationToken)
        {
            if (key == null) return false;
            await _SiteSettingService.RemoveFavIconPicture(key.Value, cancellationToken);
            return true;
        }
    }
}
