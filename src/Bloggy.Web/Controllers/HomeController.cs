using Bloggy.Service.DynamicPageService;
using Bloggy.Service.PostCategoryService;
using Bloggy.Service.PostService;
using Bloggy.Service.SiteSettingService;
using Bloggy.Shared;
using Bloggy.Shared.Model;
using Bloggy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace Bloggy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService PostService;
        private readonly IDynamicPageService DynamicPageService;
        private readonly IPostCategoryService CategoryService;
        private readonly IConfiguration Configuration;
        private readonly ISiteSettingService SiteSettingService;
        private readonly IStringLocalizer<SharedResources> Localizer;
        public HomeController(IPostService postService, IPostCategoryService categoryService, IConfiguration configuration, ISiteSettingService siteSettingService, IStringLocalizer<SharedResources> localizer, IDynamicPageService dynamicPageService)
        {
            PostService = postService;
            CategoryService = categoryService;
            Configuration = configuration;
            SiteSettingService = siteSettingService;
            Localizer = localizer;
            DynamicPageService = dynamicPageService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var setting = await SiteSettingService.GetFirstItemForEdit(cancellationToken);

            var PostList = await PostService.List(new Shared.Config.Pageres { PageNumber = 1, PageSize = 5 }, cancellationToken, IsPublished: true);

            List<PostListSelectDto> SideMenuItems = new();
            if (setting != null && setting.SideMenuCategoryId != null)
            {
                SideMenuItems = await PostService.List(new Shared.Config.Pageres { PageNumber = 1, PageSize = 5 }, cancellationToken, CategoryId: setting.SideMenuCategoryId.Value, IsPublished: true);
            }

            var catList = await CategoryService.List(cancellationToken);
            var indexItems = new IndexItemsDto
            {
                Posts = PostList,
                SideMenuItems = SideMenuItems,
                Cats = catList
            };

            if (setting != null)
            {
                indexItems.SideMenuTitle = setting.SideMenuCategoryTitle;
                ViewBag.Title = setting.SiteTitle;
                ViewBag.Description = setting.SiteDesc;
            }

            return View("~/Views/" + Configuration["Bloggy:ThemeName"] + "/Index.cshtml", indexItems);
        }

        [HttpGet("[action]/{slug}")]
        public async Task<IActionResult> Post(string slug, CancellationToken cancellationToken)
        {
            var FindedPost = await PostService.GetBySlug(slug, cancellationToken);

            ViewBag.Title = FindedPost.Title;
            ViewBag.Description = FindedPost.ShortContent;

            return View("~/Views/" + Configuration["Bloggy:ThemeName"] + "/PostDetail.cshtml", FindedPost);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PostList(CancellationToken cancellationToken, int Page = 1, int CatId = 0, string SearchText = "")
        {
            if (CatId != 0)
            {
                var FindedCategory = await CategoryService.GetById(CatId, cancellationToken);
                if (FindedCategory == null)
                    return RedirectToAction(nameof(Index));

                ViewBag.Title = Localizer["search-result"] + " " + FindedCategory.Title;
                ViewBag.Description = FindedCategory.Description;
            }
            else
                ViewBag.Title = Localizer["post-list"];

            ListModel<PostListSelectDto> PostList = new()
            {
                ItemCount = await PostService.GetTotalPostCount(CatId, IsPublished: true, SearchText),
                Items = await PostService.List(new Shared.Config.Pageres { PageNumber = Page, PageSize = 10 },
                cancellationToken, CategoryId: CatId, IsPublished: true, SearchText),
                CurrentPage = Page
            };

            if (CatId != 0)
                ViewBag.CatId = "&catId=" + CatId;

            if (!string.IsNullOrEmpty(SearchText))
                ViewBag.SearchText = "&SearchText=" + SearchText;

            return View("~/Views/" + Configuration["Bloggy:ThemeName"] + "/PostList.cshtml", PostList);
        }

        [HttpGet("[action]/{slug}")]
        public async Task<IActionResult> Page(string slug, CancellationToken cancellationToken)
        {
            var FindedItem = await DynamicPageService.GetBySlug(slug, cancellationToken);

            ViewBag.Title = FindedItem.Title;
            ViewBag.Description = FindedItem.ShortContent;

            return View("~/Views/" + Configuration["Bloggy:ThemeName"] + "/DynamicPage.cshtml", FindedItem);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}