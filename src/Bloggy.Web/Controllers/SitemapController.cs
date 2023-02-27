using Bloggy.Data.Contracts;
using Bloggy.Entities;
using Bloggy.Web.Configuration;
using Bloggy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IConfiguration _Configuration;
        private readonly IRepository<Post> _PostRepository;
        private readonly IRepository<DynamicPage> _DynamicPageRepository;

        public SitemapController(IConfiguration configuration, IRepository<Post> postRepository, IRepository<DynamicPage> dynamicPageRepository)
        {
            _Configuration = configuration;
            _PostRepository = postRepository;
            _DynamicPageRepository = dynamicPageRepository;
        }

        [Route("sitemap.xml")]
        public async Task<ActionResult> SitemapAsync()
        {
            string baseUrl = _Configuration.GetSection("Bloggy:SiteUrl").Value;

            if (baseUrl.EndsWith("/"))
                baseUrl = baseUrl.Remove(baseUrl.Length - 1);

            var posts = await _PostRepository.TableNoTracking.ToListAsync();
            var siteMapBuilder = new SitemapBuilder();
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            foreach (var post in posts)
            {
                siteMapBuilder.AddUrl(baseUrl + "/post/" + post.Slug, modified: post.CreateDate, changeFrequency: null, priority: 0.9);
            }

            var ListOfPages = await _DynamicPageRepository.TableNoTracking.ToListAsync();
            foreach (var page in ListOfPages)
            {
                siteMapBuilder.AddUrl(baseUrl + "/page/" + page.Slug, modified: page.CreateDate, changeFrequency: null, priority: 0.9);
            }

            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }
    }
}
