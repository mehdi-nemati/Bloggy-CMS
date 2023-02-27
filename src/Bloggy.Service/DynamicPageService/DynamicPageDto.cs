using AutoMapper;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using System.ComponentModel.DataAnnotations;

namespace Bloggy.Service.DynamicPageService
{
    public class DynamicPageCustomMapping : IHaveMapping
    {
        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<DynamicPage, DynamicPageListSelectDto>().ReverseMap();
            profile.CreateMap<DynamicPage, DynamicPageSelectDto>().ReverseMap();
            profile.CreateMap<DynamicPage, DynamicPageInsertDto>().ReverseMap();
        }
    }

    public class DynamicPageListSelectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string Slug { get; set; }
    }

    public class DynamicPageSelectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
        public string Slug { get; set; }
    }

    public class DynamicPageInsertDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "requierd")]
        [Display(Name = "title")]
        [MaxLength(100, ErrorMessage = "max-length")]
        public string Title { get; set; }

        [Required(ErrorMessage = "requierd")]
        [Display(Name = "short-content")]
        [MaxLength(160, ErrorMessage = "max-length")]
        public string ShortContent { get; set; }

        [Display(Name = "post-content")]
        public string Content { get; set; }

        [Display(Name = "post-slug")]
        public string Slug { get; set; }
    }
}
