using AutoMapper;
using Bloggy.Entities;
using Bloggy.Shared.Config;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bloggy.Service.MenuService
{
    public class MenuCustomMapping : IHaveMapping
    {
        public void CreateMappings(Profile profile)
        {
            profile.CreateMap<Menu, MenuWithChildrenSelecDto>().ReverseMap();
            profile.CreateMap<Menu, MenuDto>().ReverseMap();
        }
    }

    public class MenuWithChildrenSelecDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public byte Order { get; set; }
        public int? ParentId { get; set; }
        public List<MenuWithChildrenSelecDto> Children { get; set; }
    }

    public class MenuDto
    {
        public int Id { get; set; }

        public SelectList ParentCategory { get; set; }

        [Display(Name = "title")]
        [Required(ErrorMessage = "requierd")]
        [MaxLength(100, ErrorMessage = "max-length")]
        public string Title { get; set; }

        [Display(Name = "url")]
        [MaxLength(200, ErrorMessage = "max-length")]
        public string Url { get; set; }

        [Display(Name = "order")]
        public byte Order { get; set; }

        [Display(Name = "parent")]
        public int? ParentId { get; set; }
    }
}
