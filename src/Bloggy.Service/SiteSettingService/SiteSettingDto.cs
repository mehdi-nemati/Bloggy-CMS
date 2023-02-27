using AutoMapper;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggy.Service.SiteSettingService
{
    public class SiteSettingDtoCustomMapping : IHaveMapping
    {
        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<SiteSetting, SiteSettingInsertDto>().ReverseMap();
            profile.CreateMap<SiteSetting, SiteSettingSelectDto>().ReverseMap();
        }
    }

    public class SiteSettingInsertDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "requierd")]
        [Display(Name = "title")]
        [MaxLength(100, ErrorMessage = "max-length")]
        public string SiteTitle { get; set; }

        [Required(ErrorMessage = "requierd")]
        [Display(Name = "short-content")]
        [MaxLength(160, ErrorMessage = "max-length")]
        public string SiteDesc { get; set; }


        [Display(Name = "site-logo")]
        public string SiteLogo { get; set; }
        [Display(Name = "site-logo")]
        public IFormFile SiteLogoFile { get; set; }


        [Display(Name = "site-fav-icon")]
        public string SiteFavIcon { get; set; }
        [Display(Name = "site-fav-icon")]
        public IFormFile SiteFavIconFile { get; set; }

        public SelectList Categories { get; set; }

        [Display(Name = "side-menu-category-title")]
        public string SideMenuCategoryTitle { get; set; }

        [Display(Name = "side-menu-category-id")]
        public int? SideMenuCategoryId { get; set; }
    }

    public class SiteSettingSelectDto
    {
        public int Id { get; set; }
        public string SiteTitle { get; set; }
        public string SiteDesc { get; set; }
        public string SiteLogo { get; set; }
        public string SiteFavIcon { get; set; }
        public string SideMenuCategoryTitle { get; set; }
        public int? SideMenuCategoryId { get; set; }
    }
}
