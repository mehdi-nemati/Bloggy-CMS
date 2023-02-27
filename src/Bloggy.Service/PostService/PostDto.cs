using AutoMapper;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggy.Service.PostService
{
    public class PostDtoCustomMapping : IHaveMapping
    {
        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<Post, PostListSelectDto>().ReverseMap();
            profile.CreateMap<Post, PostInsertDto>().ReverseMap();
            profile.CreateMap<Post, PostSelectDto>().ReverseMap();
        }
    }

    public class PostListSelectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string CoverImage { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public string Slug { get; set; }

        public int PostCategoryId { get; set; }
        public string PostCategoryTitle { get; set; }

        public string CreatorUserDisplayName { get; set; }
    }

    public class PostInsertDto
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

        [Display(Name = "select-image")]
        public string CoverImage { get; set; }

        [Display(Name = "select-image")]
        public IFormFile CoverImageFile { get; set; }

        [Display(Name = "is-published")]
        public bool IsPublished { get; set; }

        [Display(Name = "post-slug")]
        public string Slug { get; set; }

        [Display(Name = "post-category")]
        public int PostCategoryId { get; set; }
        public SelectList PostCategories { get; set; }

        public int CreatorUserId { get; set; }
    }

    public class PostSelectDto
    {
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public DateTime CreateDate { get; set; }
        public string Content { get; set; }
        public string CoverImage { get; set; }
        public int PostCategoryId { get; set; }
        public int CreatorUserId { get; set; }
        public string PostCategoryTitle { get; set; }
        public string CreatorUserDisplayName { get; set; }
        public bool IsPublished { get; set; }
        public string Slug { get; set; }
    }
}
