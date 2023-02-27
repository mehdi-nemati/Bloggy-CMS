using AutoMapper;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using System.ComponentModel.DataAnnotations;

namespace Bloggy.Service.PostCategoryService
{
    public class PostCategoryCustomMapping : IHaveMapping
    {
        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<PostCategory, PostCategorySelectDto>().ReverseMap();
            profile.CreateMap<PostCategory, PostCategoryInsertDto>().ReverseMap();
        }
    }

    public class PostCategoryInsertDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "requierd")]
        [Display(Name = "title")]
        [MaxLength(60, ErrorMessage = "max-length")]
        public string Title { get; set; }

        [Display(Name = "description")]
        [MaxLength(160, ErrorMessage = "max-length")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }

    public class PostCategorySelectDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
