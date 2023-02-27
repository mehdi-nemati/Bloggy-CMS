using Bloggy.Service.AlertService;
using Bloggy.Service.PostCategoryService;
using Bloggy.Service.PostService;
using Bloggy.Service.UploadService;
using Bloggy.Shared.Config;
using Bloggy.Shared.Extension;
using Bloggy.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggy.Web.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _PostService;
        private readonly IPostCategoryService _PostCategoryService;
        private readonly IUploadService _UploadService;
        public PostController(IAlertService alertService, IPostService PostService, IPostCategoryService postCategoryService, IUploadService uploadService) : base(alertService)
        {
            _PostService = PostService;
            _PostCategoryService = postCategoryService;
            _UploadService = uploadService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken, int Page = 1)
        {
            ListModel<PostListSelectDto> PostList = new()
            {
                ItemCount = await _PostService.GetTotalPostCount(),
                Items = await _PostService.List(new Pageres { PageNumber = Page, PageSize = 10 },
                        cancellationToken),
                CurrentPage = Page
            };

            return View(PostList);
        }

        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var PostDto = new PostInsertDto
            {
                PostCategories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title")
            };
            return View("add", PostDto);
        }

        public async Task<IActionResult> Update(int Id, CancellationToken cancellationToken)
        {
            var FindedItem = await _PostService.GetByIdForEdit(Id, cancellationToken);
            FindedItem.PostCategories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title", FindedItem.PostCategoryId);
            return View("add", FindedItem);
        }

        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            await _PostService.Delete(Id, cancellationToken);
            CreateSuccessAlert();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Submit(PostInsertDto dto, CancellationToken cancellationToken)
        {
            dto.CreatorUserId = User.Identity.GetUserIdInt();
            var res = await _PostService.Submit(dto, cancellationToken);
            if (res.IsSuccess)
            {
                CreateSuccessAlert();
                return RedirectToAction(nameof(Index));
            }
            dto.PostCategories = new SelectList(await _PostCategoryService.List(cancellationToken), "Id", "Title", dto.PostCategoryId);
            CreateAlert("", res.Message, AlertIcons.error);
            return View("add", dto);
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

            var url = await _UploadService.UploadFile(upload, "CKEditorImages");
            var success = new { uploaded = 1, url, error = new { message = "image is uploaded" } };

            return Json(success);
        }

        [HttpPost]
        public async Task<bool> DeleteFileUpload(int? key, CancellationToken cancellationToken)
        {
            if (key == null) return false;
            await _PostService.RemovePicture(key.Value, cancellationToken);
            return true;
        }
    }
}
